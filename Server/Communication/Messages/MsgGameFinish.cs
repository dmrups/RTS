using System.Collections.Generic;

namespace Server.Communication.Messages
{
    internal class MsgGameFinish : Message
    {
        public override MessageCode? Code { get; } = MessageCode.GameFinish;
        public IDictionary<int, float> ScoreTable;

        public MsgGameFinish(IDictionary<int, float> scoreTable)
        {
            ScoreTable = scoreTable;
        }
    }
}