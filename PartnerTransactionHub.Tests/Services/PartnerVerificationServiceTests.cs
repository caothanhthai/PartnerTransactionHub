using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTSCo.Services;
using Xunit;

namespace TTSCom.Tests.Services
{
    public class PartnerVerificationServiceTests
    {
        private HttpClient CreateHttpClient(Func<HttpResponseMessage> responseFunc)
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseFunc);

            return new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }

        [Fact]
        public async Task Should_Return_True_When_Status_OK()
        {
            var client = CreateHttpClient(() => new HttpResponseMessage(HttpStatusCode.OK));
            var logger = new Mock<ILogger<PartnerVerificationService>>();

            var service = new PartnerVerificationService(client, logger.Object);

            var result = await service.VerifyPartnerAsync("P1");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Return_False_When_Status_Not_OK()
        {
            var client = CreateHttpClient(() => new HttpResponseMessage(HttpStatusCode.BadRequest));
            var logger = new Mock<ILogger<PartnerVerificationService>>();

            var service = new PartnerVerificationService(client, logger.Object);

            var result = await service.VerifyPartnerAsync("P1");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Return_False_When_Exception()
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException());

            var client = new HttpClient(handler.Object);
            var logger = new Mock<ILogger<PartnerVerificationService>>();

            var service = new PartnerVerificationService(client, logger.Object);

            var result = await service.VerifyPartnerAsync("P1");

            result.Should().BeFalse();
        }

        //[Fact]
        //public async Task Should_Retry_On_Failure_And_Succeed()
        //{
        //    var handlerMock = new Mock<HttpMessageHandler>();

        //    int callCount = 0;

        //    handlerMock.Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //            "SendAsync",
        //            ItExpr.IsAny<HttpRequestMessage>(),
        //            ItExpr.IsAny<CancellationToken>())
        //        .ReturnsAsync(() =>
        //        {
        //            while (callCount < 3)
        //                callCount++;

        //            return new HttpResponseMessage(HttpStatusCode.OK);
        //        });

        //    var httpClient = new HttpClient(handlerMock.Object)
        //    {
        //        BaseAddress = new Uri("http://localhost")
        //    };

        //    var loggerMock = new Mock<ILogger<PartnerVerificationService>>();

        //    var service = new PartnerVerificationService(httpClient, loggerMock.Object);

        //    var result = await service.VerifyPartnerAsync("P1");

        //    result.Should().BeTrue();
        //    callCount.Should().BeGreaterOrEqualTo(3);
        //}
    }
}
