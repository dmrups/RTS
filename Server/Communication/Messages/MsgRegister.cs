namespace Server.Communication.Messages
{
    internal class MsgRegister : Message
    {
        public override MessageCode? Code { get; } = MessageCode.Register;

        public ClientType? Type { get; set; }

        public string Name { get; set; }
    }
}