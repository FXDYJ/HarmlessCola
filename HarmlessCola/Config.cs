using System.ComponentModel;

namespace HarmlessCola
{
    /// <summary>
    /// Configuration class for HarmlessCola plugin
    /// Contains all configurable options to customize plugin behavior
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets whether the plugin is enabled
        /// </summary>
        [Description("Is the plugin enabled?")]
        public bool PluginEnable { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to display normal logs
        /// Used to record basic operation information of the plugin
        /// </summary>
        [Description("Do you want to display the log?")]
        public bool LogEnable { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to display debug logs
        /// Debug logs contain more detailed information, mainly used for development and troubleshooting
        /// </summary>
        [Description("Do you want to display the debug log? (Suggest not displaying it as it is redundant and useless)")]
        public bool DebugLogEnable { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to send messages when players use SCP-207
        /// </summary>
        [Description("Do you want to send a message when a player uses SCP-207?")]
        public bool MessageEnable { get; set; } = true;

        /// <summary>
        /// Gets or sets the message display type
        /// Supports "Broadcast" and "Hint" types
        /// </summary>
        [Description("How to display this message? (Broadcast/Hint)")]
        public string MessageType { get; set; } = "Broadcast";

        /// <summary>
        /// Gets or sets the message content to send
        /// You can use [player] placeholder to display player name
        /// </summary>
        [Description("What message do you want to send? (Use [player] for the player's name)")]
        public string MessageContent { get; set; } = "Our server has enabled Coke No Harm, CHARGE!!!";

        /// <summary>
        /// Gets or sets the message display duration (seconds)
        /// Only effective for Broadcast and Hint type messages
        /// </summary>
        [Description("How long do you want the message to be displayed in seconds?")]
        public ushort MessageDuration { get; set; } = 3;

        /// <summary>
        /// Validates whether the configuration is valid
        /// </summary>
        /// <returns>Returns true if configuration is valid, otherwise false</returns>
        public bool IsValid()
        {
            // Check if message type is supported
            if (MessageEnable && !IsValidMessageType(MessageType))
                return false;

            // Check if message content is empty
            if (MessageEnable && string.IsNullOrWhiteSpace(MessageContent))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the message type is valid
        /// </summary>
        /// <param name="messageType">The message type to check</param>
        /// <returns>Returns true if message type is valid</returns>
        private static bool IsValidMessageType(string messageType)
        {
            return messageType is "Broadcast" or "Hint";
        }
    }
}