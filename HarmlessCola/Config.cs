using System.ComponentModel;

namespace HarmlessCola;

public class Config
{
    [Description("Is the plugin enabled?")]
    public bool PluginEnable { get; set; } = true;
    
    [Description("Do you want to display the log?")]
    public bool LogEnable { get; set; } = true;
    
    [Description("Do you want to display the debug log? (Suggest not displaying it as it is redundant and useless)")]
    public bool DebugLogEnable { get; set; } = false;
    
    [Description("Do you want to send a message when a player uses SCP-207?")]
    public bool MessageEnable { get; set; } = true;
    
    [Description("How to display this message? (Broadcast/Hint)")]
    public string MessageType { get; set; } = "Broadcast";
    
    [Description("What message do you want to send? (Use [player] for the player's name)")]
    public string MessageContent { get; set; } = "Our server has enabled Coke No Harm, CHARGE!!!";
    
    [Description("How long do you want the message to be displayed in seconds?")]
    public ushort MessageDuration { get; set; } = 3;
}