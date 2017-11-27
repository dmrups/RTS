using Server.Interfaces.Entities;

namespace Server.Communication.Messages
{
    internal class MsgGameState : Message
    {
        public override MessageCode? Code { get; } = MessageCode.GameState;
        public IGame Game { get; set; }

        public MsgGameState(IGame game)
        {
            Game = game;
        }
    }
}