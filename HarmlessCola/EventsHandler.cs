using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using PlayerStatsSystem;

namespace HarmlessCola
{
    /// <summary>
    /// Event handler for SCP-207 damage exemption
    /// This class is responsible for intercepting and preventing damage caused by SCP-207
    /// </summary>
    public class ExemptDamageEventsHandler : CustomEventsHandler
    {
        /// <summary>
        /// Event handling method triggered when a player takes damage
        /// Checks if the damage source is SCP-207, and if so, prevents the damage
        /// </summary>
        /// <param name="ev">Event arguments containing player injury information</param>
        public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            // Check if the damage handler is a universal damage handler
            if (ev.DamageHandler is not UniversalDamageHandler damage) 
                return;

            // Check if the damage translation ID is from SCP-207
            if (damage.TranslationId != DeathTranslations.Scp207.Id) 
                return;

            // Prevent SCP-207 damage
            ev.IsAllowed = false;

            // Log debug information (if debug logging is enabled)
            Logger.Debug(
                $"Player {ev.Player.Nickname} is exempt from damage from SCP-207",
                HarmlessColaPlugin.Config.DebugLogEnable
            );
        }
    }

    /// <summary>
    /// Event handler for message sending
    /// Sends configured reminder messages when players use SCP-207
    /// </summary>
    public class SendMessageEventsHandler : CustomEventsHandler
    {
        /// <summary>
        /// Event handling method triggered when a player uses an item
        /// Checks if the used item is SCP-207, and if so, sends the configured message
        /// </summary>
        /// <param name="ev">Event arguments containing player item usage information</param>
        public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
        {
            // Check if the used item is SCP-207
            if (ev.UsableItem.Type != ItemType.SCP207) 
                return;

            // Log player SCP-207 usage information (if logging is enabled)
            if (HarmlessColaPlugin.Config.LogEnable)
            {
                Logger.Info($"Player {ev.Player.Nickname} used SCP-207");
            }

            // If message functionality is enabled, send message
            if (!HarmlessColaPlugin.Config.MessageEnable) 
                return;

            // Process placeholders in message content
            string processedMessage = ProcessMessageContent(
                HarmlessColaPlugin.Config.MessageContent, 
                ev.Player.Nickname
            );

            // Send appropriate message based on configured message type
            SendMessageToPlayer(ev.Player, processedMessage);
        }

        /// <summary>
        /// Processes placeholders in message content
        /// </summary>
        /// <param name="messageContent">Original message content</param>
        /// <param name="playerNickname">Player nickname</param>
        /// <returns>Processed message content</returns>
        private static string ProcessMessageContent(string messageContent, string playerNickname)
        {
            // Replace [player] placeholder with actual player nickname
            return messageContent.Replace("[player]", playerNickname);
        }

        /// <summary>
        /// Sends message to player based on configured message type
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="message">Message to send</param>
        private static void SendMessageToPlayer(LabApi.Features.Wrappers.Player player, string message)
        {
            switch (HarmlessColaPlugin.Config.MessageType)
            {
                case "Broadcast":
                    // Send broadcast message
                    player.SendBroadcast(message, HarmlessColaPlugin.Config.MessageDuration);
                    break;

                case "Hint":
                    // Send hint message
                    player.SendHint(message, HarmlessColaPlugin.Config.MessageDuration);
                    break;

                default:
                    // Log error when configuration is invalid
                    Logger.Error(
                        $"Invalid MessageType '{HarmlessColaPlugin.Config.MessageType}' in config. " +
                        "Please set it to 'Broadcast' or 'Hint'."
                    );
                    break;
            }
        }
    }
}