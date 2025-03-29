using Microsoft.Extensions.DependencyInjection;
using SharedLibraryCore;
using SharedLibraryCore.Events.Management;
using SharedLibraryCore.Helpers;
using SharedLibraryCore.Interfaces;
using SharedLibraryCore.Interfaces.Events;
using SharedLibraryCore.Database.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Warn
{
    public class Plugin : IPluginV2
    {
        public const string PluginName = "WarnPlugin";
        public string Name => PluginName;
        public string Version => "2025-03-29";
        public string Author => "HGM";

        private readonly IInteractionRegistration _interactionRegistration;
        private readonly IRemoteCommandService _remoteCommandService;
        private const string WarnInteractionId = "Webfront::Profile::Warn";

        public Plugin(
            IInteractionRegistration interactionRegistration,
            IRemoteCommandService remoteCommandService)
        {
            _interactionRegistration = interactionRegistration;
            _remoteCommandService = remoteCommandService;

            IManagementEventSubscriptions.Load += OnLoad;
        }

        public static void RegisterDependencies(IServiceCollection services) { }

        private Task OnLoad(IManager manager, CancellationToken token)
        {
            _interactionRegistration.RegisterInteraction(WarnInteractionId, async (targetClientId, game, _) =>
            {
                if (!targetClientId.HasValue)
                    return null;

                var server = manager.GetServers().FirstOrDefault();

                var reasonInput = new
                {
                    Name = "Reason",
                    Label = "Reason",
                    Type = "text",
                    Values = (Dictionary<string, string>?)null
                };

                var presetReasonInput = new
                {
                    Name = "PresetReason",
                    Label = "Preset Reason",
                    Type = "select",
                    Values = new Dictionary<string, string>
                    {
                        { string.Empty, string.Empty },
                        { "Spamming", "Spamming" },
                        { "Disrespect", "Disrespect" },
                        { "Racism", "Racism" },
                        { "Excessive Trolling", "Excessive Trolling" },
                        { "Toxic Behavior", "Toxic Behavior" },
                        { "AFK", "AFK" }
                    }
                };

                var inputs = new[] { reasonInput, presetReasonInput };
                var inputsJson = JsonSerializer.Serialize(inputs);

                return new InteractionData
                {
                    EntityId = targetClientId.Value,
                    Name = "Warn",
                    DisplayMeta = "oi-warning",
                    ActionPath = "DynamicAction",
                    ActionMeta = new Dictionary<string, string>
                    {
                        { "InteractionId", WarnInteractionId },
                        { "Inputs", inputsJson },
                        { "ActionButtonLabel", "Warn" },
                        { "Name", "Warn Player" },
                        { "ShouldRefresh", false.ToString() }
                    },
                    MinimumPermission = EFClient.Permission.Moderator,
                    Source = Name,
                    Action = async (originId, targetId, gameName, meta, cancellationToken) =>
                    {
                        if (!targetId.HasValue)
                            return "No target client specified.";

                        var reason = meta.TryGetValue("Reason", out var reasonText) ? reasonText : string.Empty;
                        if (string.IsNullOrWhiteSpace(reason) && meta.TryGetValue("PresetReason", out var preset))
                        {
                            reason = preset;
                        }

                        var result = await _remoteCommandService.Execute(originId, targetId, "warn", new List<string> { reason }, server);
                        return string.Join("\n", result.Select(r => r.Response));
                    }
                };
            });

            Console.WriteLine($"[{Name}] by {Author} loaded. Version: {Version}");
            return Task.CompletedTask;
        }
    }
}
