using System;
using LabApi.Events;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;

namespace HarmlessCola
{
    public class HarmlessColaPlugin : Plugin
    {
        public static HarmlessColaPlugin Instance { get; private set; }

        public override string Name => "HarmlessCola";

        public override string Description => "Ensure that Coca Cola (SCP-207) no longer causes harm";

        public override string Author => "FlyCloud";

        public override Version Version => new(1,0,0,0);

        public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

        public static Config Config;
        
        private bool _hasIncorrectSettings;
        
        public ExemptDamageEventsHandler ExemptDamageEvent { get; } = new ();
        
        public SendMessageEventsHandler SendMessageEvent { get; } = new ();
        
        public override void LoadConfigs()
        {
            base.LoadConfigs();
            _hasIncorrectSettings = !this.TryLoadConfig("Config.yml", out Config);
            if(_hasIncorrectSettings || Config == null) return;

            if (Config.MessageType == "Broadcast" || Config.MessageType == "Hint" || !Config.MessageContent.IsEmpty()) return;
            Logger.Error("MessageType is set incorrectly in the config file. Please set it to 'Broadcast' or 'Hint'.");
            _hasIncorrectSettings = true;
        }

        public override void Enable()
        {
            if (_hasIncorrectSettings)
            {
                Logger.Error("Config format error! The plugin will NOT load");
                return;
            }

            Instance = this;

            if (Config.PluginEnable)
            {
                // Register events
                CustomHandlersManager.RegisterEventsHandler(ExemptDamageEvent);
                if (Config.MessageEnable) CustomHandlersManager.RegisterEventsHandler(SendMessageEvent);
                
                // Just to show off :)
                // You can use this website to create your own: https://patorjk.com/software/taag/
                Logger.Raw("""
                            _    _                      _                _____      _       
                           | |  | |                    | |              / ____|    | |      
                           | |__| | __ _ _ __ _ __ ___ | | ___  ___ ___| |     ___ | | __ _ 
                           |  __  |/ _` | '__| '_ ` _ \| |/ _ \/ __/ __| |    / _ \| |/ _` |
                           | |  | | (_| | |  | | | | | | |  __/\__ \__ \ |___| (_) | | (_| |
                           |_|  |_|\__,_|_|  |_| |_| |_|_|\___||___/___/\_____\___/|_|\__,_|
                                                                                            
                           HarmlessCola plugin has been loaded!                                                         
                           """, ConsoleColor.Cyan);
            }
        }
        
        public override void Disable()
        {
            // Is this thing really work?
            Logger.Info("HarmlessCola plugin has been disabled!");
        }
    }
}
