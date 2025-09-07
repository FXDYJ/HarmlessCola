using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using PlayerStatsSystem;

namespace HarmlessCola;

public class ExemptDamageEventsHandler : CustomEventsHandler
{
    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if (ev.DamageHandler is not UniversalDamageHandler damage) return;
        if (damage.TranslationId != DeathTranslations.Scp207.Id) return;
        ev.IsAllowed = false;
        Logger.Debug($"Player {ev.Player.Nickname} is exempt from damage from SCP-207",
            HarmlessColaPlugin.Config.DebugLogEnable);
    }
}

public class SendMessageEventsHandler : CustomEventsHandler
{
    public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
    {
        if (ev.UsableItem.Type == ItemType.SCP207)
        {
            if (HarmlessColaPlugin.Config.LogEnable) Logger.Info($"Player {ev.Player.Nickname} used SCP-207");
            
            switch (HarmlessColaPlugin.Config.MessageType)
            {
                case "Broadcast":
                    ev.Player.SendBroadcast(HarmlessColaPlugin.Config.MessageContent, HarmlessColaPlugin.Config.MessageDuration);
                    break;
                case "Hint":
                    ev.Player.SendHint(HarmlessColaPlugin.Config.MessageContent, HarmlessColaPlugin.Config.MessageDuration);
                    break;
                default:
                    Logger.Error("MessageType is set incorrectly in the config file. Please set it to 'Broadcast' or 'Hint'.");
                    break;
            }
        }
    }
}