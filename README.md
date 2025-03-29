
# WarnPlugin

🔧 **WarnPlugin** is a plugin for [IW4MAdmin](https://github.com/RaidMax/IW4M-Admin) that adds a "Warn" button to player profiles on the webfront. This plugin was originally created for **HGMServers**, but is open-source and available for anyone to use or modify. It allows server staff to issue in-game warnings using the `!warn` command, with support for both custom and preset reasons.

---

## ✨ Features

- Adds a **"Warn"** button to the webfront player context menu
- Supports **preset and custom reasons**
- Executes `!warn <player> <reason>` from the web interface
- Permission-restricted to **Moderators and above**

---

## 🧱 Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [IW4MAdmin](https://github.com/RaidMax/IW4M-Admin)
- [SharedLibraryCore NuGet package](https://www.nuget.org/packages/RaidMax.IW4MAdmin.SharedLibraryCore)

---

## 🛠️ Build Instructions

```bash
# Clone the repo
git clone https://github.com/NotHGM/WarnPlugin.git
cd WarnPlugin

# Restore dependencies & build
dotnet restore
dotnet build -c Release
```

After building, the compiled plugin will be located at:

```
bin/Release/net8.0/WarnPlugin.dll
```

Copy `WarnPlugin.dll` into your `IW4MAdmin/Plugins/` directory and restart IW4MAdmin.

---

## 🧪 Example Output on Startup

```
[WarnPlugin] by HGM loaded. Version: 2025-03-29
```

---

## 🙌 Credits

Special thanks to:

- [RaidMax](https://github.com/RaidMax) for creating IW4MAdmin 🛠️
- [Amos](https://github.com/ayymoss) for the inspiration 💡
