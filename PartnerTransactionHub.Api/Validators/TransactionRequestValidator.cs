using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TTSCo.Models;

namespace TTSCo.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(x => x.PartnerId).NotEmpty();
            RuleFor(x => x.TransactionReference).NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Currency)
                .Must(BeValidCurrency)
                .WithMessage("Invalid currency code");

            RuleFor(x => x.Timestamp)
                .NotEmpty();
        }

        private bool BeValidCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                return false;

            // Get all specific cultures
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            // Check if any region uses this currency code
            return cultures.Select(culture => {
                try
                {
                    return new RegionInfo(culture.Name).ISOCurrencySymbol;
                }
                catch
                {
                    return null;
                }
            }).Any(iso => iso != null && iso.Equals(currency, StringComparison.OrdinalIgnoreCase));
        }
    }
}
