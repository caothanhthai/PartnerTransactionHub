using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTSCom.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message);
    }
}
