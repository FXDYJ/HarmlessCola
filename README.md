# HarmlessCola Plugin

A plugin for SCP: Secret Laboratory servers that makes SCP-207 (Cola) no longer cause harm to players.

## Features

- 🥤 Remove SCP-207 damage effects
- 💬 Configurable player usage message reminders
- 🛠️ Flexible configuration options
- 📝 Detailed logging system

## Installation

1. Download the latest plugin file
2. Place `HarmlessCola.dll` in your server's plugins directory (Usually, it's C:\Users\Administrator\AppData\Roaming\SCP Secret Laboratory\LabAPI\plugins\{your port / global})
3. Restart the server
4. Edit the configuration file `Config.yml` (optional)

## Configuration Options

| Configuration | Default Value | Description |
|---------------|---------------|-------------|
| `PluginEnable` | `true` | Whether to enable the plugin |
| `LogEnable` | `true` | Whether to display logs |
| `DebugLogEnable` | `false` | Whether to display debug logs |
| `MessageEnable` | `true` | Whether to send messages when players use SCP-207 |
| `MessageType` | `"Broadcast"` | Message display method (Broadcast/Hint) |
| `MessageContent` | `"Our server has enabled Coke No Harm, CHARGE!!!"` | Message content to send |
| `MessageDuration` | `3` | Message display duration (seconds) |

## Development Environment

- .NET Framework 4.8
- LabApi (SCP:SL plugin framework)

## Contributing

Issues and Pull Requests are welcome!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details