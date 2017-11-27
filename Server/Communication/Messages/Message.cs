using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Server.Communication.Messages
{
    internal abstract class Message : EventArgs
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual MessageCode? Code { get; } = null;
    }
}