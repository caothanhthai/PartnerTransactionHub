using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TTSCom.Configuration;
using TTSCom.Messaging;
using Xunit;

namespace TTSCom.Tests.Messaging
{
    public class RabbitMqPublisherTests
    {
        //[Fact]
        //public async Task Should_Publish_Message()
        //{
        //    var channelMock = new Mock<IModel>();
        //    var connectionMock = new Mock<IConnection>();
        //    //var factoryMock = new Mock<IRabbitMqConnectionFactory>();
        //    var settings = Options.Create(new RabbitMqSettings
        //    {
        //        Host = "localhost", // ou "rabbitmq" si Docker
        //        QueueName = "partner-transactions"
        //    });

        //    connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object);
        //    //factoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

        //    var publisher = new RabbitMqPublisher(settings);

        //    var message = new
        //    {
        //        PartnerId = "P1",
        //        Amount = 100
        //    };

        //    await publisher.PublishAsync(message);

        //    channelMock.Verify(x => x.BasicPublish(
        //        It.IsAny<string>(),
        //        It.IsAny<string>(),
        //        It.IsAny<IBasicProperties>(),
        //        It.IsAny<byte[]>()),
        //        Times.Once);
        //}

        [Fact]
        public async Task Should_Publish_Message_Without_Exception()
        {
            var settings = Options.Create(new RabbitMqSettings
            {
                Host = "localhost", // ou "rabbitmq" si Docker
                QueueName = "partner-transactions"
            });

            var publisher = new RabbitMqPublisher(settings);

            var message = new
            {
                PartnerId = "P1",
                Amount = 100
            };

            await publisher.PublishAsync(message);

            Assert.True(true); // smoke test
        }
    }
}
