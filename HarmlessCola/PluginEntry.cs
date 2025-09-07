using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;

namespace HarmlessCola
{
    /// <summary>
    /// Main entry class for HarmlessCola plugin
    /// Inherits from Plugin base class, responsible for managing plugin lifecycle, configuration loading and event registration
    /// </summary>
    public class HarmlessColaPlugin : Plugin
    {
        #region Plugin Information
        
        /// <summary>
        /// Static instance of the plugin for global access
        /// Uses singleton pattern to ensure only one plugin instance
        /// </summary>
        public static HarmlessColaPlugin Instance { get; private set; }

        /// <summary>
        /// Plugin name
        /// </summary>
        public override string Name => "HarmlessCola";

        /// <summary>
        /// Plugin description
        /// </summary>
        public override string Description => "Ensure that Coca Cola (SCP-207) no longer causes harm to players";

        /// <summary>
        /// Plugin author
        /// </summary>
        public override string Author => "FlyCloud";

        /// <summary>
        /// Plugin version
        /// Uses semantic versioning: major.minor.patch.build
        /// </summary>
        public override Version Version => new(1, 0, 0, 0);

        /// <summary>
        /// Required LabApi version
        /// </summary>
        public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

        #endregion

        #region Configuration and State

        /// <summary>
        /// Plugin configuration instance containing all configurable options
        /// </summary>
        public static Config Config { get; private set; }
        
        /// <summary>
        /// Flag indicating if configuration has errors
        /// If configuration file format is wrong or validation fails, this flag will be true
        /// </summary>
        private bool _hasConfigurationErrors;

        #endregion

        #region Event Handlers

        /// <summary>
        /// SCP-207 damage exemption event handler
        /// Responsible for intercepting and preventing SCP-207 damage to players
        /// </summary>
        public ExemptDamageEventsHandler ExemptDamageEventHandler { get; } = new();
        
        /// <summary>
        /// Message sending event handler
        /// Responsible for sending configured messages when players use SCP-207
        /// </summary>
        public SendMessageEventsHandler SendMessageEventHandler { get; } = new();

        #endregion

        #region Plugin Lifecycle Methods

        /// <summary>
        /// Load plugin configuration
        /// Called before plugin is enabled, responsible for reading and validating configuration file
        /// </summary>
        public override void LoadConfigs()
        {
            base.LoadConfigs();
            
            try
            {
                // Attempt to load configuration file
                Config tempConfig;
                _hasConfigurationErrors = !this.TryLoadConfig("Config.yml", out tempConfig);
                Config = tempConfig;
                
                if (_hasConfigurationErrors || Config == null)
                {
                    Logger.Error("Failed to load configuration file 'Config.yml'");
                    return;
                }

                // Validate configuration validity
                if (!Config.IsValid())
                {
                    Logger.Error("Configuration validation failed. Please check your config settings.");
                    _hasConfigurationErrors = true;
                    return;
                }

                Logger.Info("Configuration loaded successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred while loading configuration: {ex.Message}");
                _hasConfigurationErrors = true;
            }
        }

        /// <summary>
        /// Enable plugin
        /// Register event handlers and initialize plugin functionality
        /// </summary>
        public override void Enable()
        {
            // Check if configuration has errors
            if (_hasConfigurationErrors)
            {
                Logger.Error("Plugin cannot be enabled due to configuration errors!");
                return;
            }

            // Check if plugin is enabled in configuration
            if (!Config.PluginEnable)
            {
                Logger.Info("Plugin is disabled in configuration.");
                return;
            }

            try
            {
                // Set plugin instance
                Instance = this;

                // Register core event handlers (SCP-207 damage exemption)
                RegisterEventHandlers();

                // Display enable success message
                DisplayWelcomeMessage();
                
                Logger.Info("HarmlessCola plugin has been enabled successfully!");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to enable plugin: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Disable plugin
        /// Clean up resources and unregister event handlers
        /// </summary>
        public override void Disable()
        {
            try
            {
                // Unregister event handlers
                UnregisterEventHandlers();
                
                // Clear instance reference
                Instance = null;
                
                Logger.Info("HarmlessCola plugin has been disabled successfully!");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error occurred while disabling plugin: {ex.Message}");
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Register event handlers
        /// Selectively register different event handlers based on configuration
        /// </summary>
        private void RegisterEventHandlers()
        {
            // Always register damage exemption handler (this is the core functionality of the plugin)
            CustomHandlersManager.RegisterEventsHandler(ExemptDamageEventHandler);
            Logger.Debug("Damage exemption event handler registered.", Config.DebugLogEnable);

            // Register message sending handler based on configuration
            if (Config.MessageEnable)
            {
                CustomHandlersManager.RegisterEventsHandler(SendMessageEventHandler);
                Logger.Debug("Message sending event handler registered.", Config.DebugLogEnable);
            }
        }

        /// <summary>
        /// Unregister event handlers
        /// Clean up registered event handlers when plugin is disabled
        /// </summary>
        private void UnregisterEventHandlers()
        {
            try
            {
                // Unregister damage exemption handler
                CustomHandlersManager.UnregisterEventsHandler(ExemptDamageEventHandler);
                
                // Unregister message sending handler
                CustomHandlersManager.UnregisterEventsHandler(SendMessageEventHandler);
                
                Logger.Debug("All event handlers have been unregistered.", Config?.DebugLogEnable ?? false);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to unregister event handlers: {ex.Message}");
            }
        }

        /// <summary>
        /// Display welcome message when plugin is enabled
        /// Contains ASCII art and version information
        /// </summary>
        private void DisplayWelcomeMessage()
        {
            // ASCII art generation tool: https://patorjk.com/software/taag/
            string welcomeMessage = $@"
 _    _                      _                _____      _       
| |  | |                    | |              / ____|    | |      
| |__| | __ _ _ __ _ __ ___ | | ___  ___ ___| |     ___ | | __ _ 
|  __  |/ _` | '__| '_ ` _ \| |/ _ \/ __/ __| |    / _ \| |/ _` |
| |  | | (_| | |  | | | | | | |  __/\__ \__ \ |___| (_) | | (_| |
|_|  |_|\__,_|_|  |_| |_| |_|_|\___||___/___/\_____\___/|_|\__,_|

HarmlessCola Plugin v{Version.ToString(3)} - Loaded Successfully!
Author: {Author}

🥤 SCP-207 damage has been disabled!
💬 Message notifications: {(Config.MessageEnable ? "ON" : "OFF")}
📝 Logging enabled: {(Config.LogEnable ? "ON" : "OFF")}
🔧 Debug logging: {(Config.DebugLogEnable ? "ON" : "OFF")}";

            Logger.Raw(welcomeMessage, ConsoleColor.Cyan);
        }

        #endregion
    }
}
