using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTSCo.Controllers;
using TTSCo.Models;
using TTSCo.Services;
using TTSCom.Messaging;
using Xunit;

namespace PartnerTransactionHub.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        [Fact]
        public async Task Should_Return_BadRequest_When_Partner_Invalid()
        {
            var verificationMock = new Mock<IPartnerVerificationService>();
            var publisherMock = new Mock<IMessagePublisher>();

            verificationMock.Setup(x => x.VerifyPartnerAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new TransactionsController(
                verificationMock.Object,
                publisherMock.Object);

            var result = await controller.CreateTransaction(new TransactionRequest());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Should_Return_Accepted_When_Valid()
        {
            var verificationMock = new Mock<IPartnerVerificationService>();
            var publisherMock = new Mock<IMessagePublisher>();

            verificationMock.Setup(x => x.VerifyPartnerAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new TransactionsController(
                verificationMock.Object,
                publisherMock.Object);

            var result = await controller.CreateTransaction(new TransactionRequest
            {
                PartnerId = "P1",
                TransactionReference = "TXN1",
                Amount = 100,
                Currency = "USD",
                Timestamp = System.DateTime.UtcNow
            });

            result.Should().BeOfType<AcceptedResult>();

            publisherMock.Verify(x => x.PublishAsync(It.IsAny<object>()), Times.Once);
        }
    }
}
