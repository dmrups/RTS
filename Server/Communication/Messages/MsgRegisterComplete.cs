namespace Server.Communication.Messages
{
    internal class MsgRegisterComplete : Message
    {
        public override MessageCode? Code { get; } = MessageCode.Register;

        public int PlayerId { get; set; }

        public MsgRegisterComplete(int playerId)
        {
            PlayerId = playerId;
        }
    }
}