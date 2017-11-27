using System.Runtime.Serialization;

namespace Server.Communication.Messages
{
    enum MessageCode
    {
        [EnumMember(Value = "register")]
        Register,
        [EnumMember(Value = "gameState")]
        GameState,
        [EnumMember(Value = "gameFinish")]
        GameFinish
    }
}