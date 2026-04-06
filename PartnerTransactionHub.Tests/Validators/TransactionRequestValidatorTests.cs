using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TTSCo.Models;
using TTSCo.Validators;
using Xunit;

namespace TTSCom.Tests.Validators
{
    public class TransactionRequestValidatorTests
    {
        private readonly TransactionRequestValidator _validator = new();

        private TransactionRequest ValidModel() => new()
        {
            PartnerId = "P1",
            TransactionReference = "TXN1",
            Amount = 100,
            Currency = "USD",
            Timestamp = DateTime.UtcNow
        };

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var result = _validator.Validate(ValidModel());
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Amount_Zero()
        {
            var model = ValidModel();
            model.Amount = 0;

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Currency_Invalid()
        {
            var model = ValidModel();
            model.Currency = "XXX";

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_PartnerId_Empty()
        {
            var model = ValidModel();
            model.PartnerId = "";

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Reference_Empty()
        {
            var model = ValidModel();
            model.TransactionReference = "";

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
        }
    }
}
