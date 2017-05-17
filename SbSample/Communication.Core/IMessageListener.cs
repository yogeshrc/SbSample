using System;
using System.Collections.Generic;

namespace Communication.Core
{
    public interface IMessageListener<TMessageType>
    {
        event EventHandler<MessageReceivedEventArgs<TMessageType>> MessageReceived;
        void Initialize(Dictionary<string, dynamic> configurations);
    }
}
