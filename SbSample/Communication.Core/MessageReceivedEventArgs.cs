using System;

namespace Communication.Core
{
    public class MessageReceivedEventArgs<TMessageType>: EventArgs
    {
        public TMessageType Message { get; set; }
    }
}