using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTSCom.Configuration
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string QueueName { get; set; }
    }
}
