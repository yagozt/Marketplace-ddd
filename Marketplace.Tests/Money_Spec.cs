using Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Marketplace.Domain.Money;

namespace Marketplace.Tests
{
    public class FakeCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<CurrencyDetails> _currencies = new[]{
            new CurrencyDetails{
                CurrencyCode = "EUR",
                DecimalPlaces = 2,
                InUse = true
            },
            new CurrencyDetails{
                CurrencyCode = "USD",
                DecimalPlaces = 2,
                InUse = true
            },
            new CurrencyDetails{
                CurrencyCode = "JPY",
                DecimalPlaces = 0,
                InUse = true
            },
            new CurrencyDetails{
                CurrencyCode = "DEM",
                DecimalPlaces = 2,
                InUse = false
            }
        };
        public CurrencyDetails FindCurrency(string currencyCode)
        {
            var currency = _currencies.FirstOrDefault(c => c.CurrencyCode == currencyCode);
            return currency ?? CurrencyDetails.None;
        }
    }
    public class Money_Spec
    {
        private static readonly ICurrencyLookup currencyLookup = new FakeCurrencyLookup();
        [Fact]
        public void Two_of_same_amount_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = Money.FromDecimal(5, "EUR", currencyLookup);

            Assert.Equal(firstAmount, secondAmount);
        }
        [Fact]
        public void Two_of_same_amount_but_differentCurrencies_should_not_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = Money.FromDecimal(5, "USD", currencyLookup);

            Assert.NotEqual(firstAmount, secondAmount);
        }
        [Fact]
        public void FromString_and_FromDecimal_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = Money.FromString("5,00", "EUR", currencyLookup);

            Assert.Equal(firstAmount, secondAmount);
        }
        [Fact]
        public void Sum_of_money_gives_full_amount()
        {

            var coin1 = Money.FromDecimal(1, "EUR", currencyLookup);
            var coin2 = Money.FromDecimal(2, "EUR", currencyLookup);
            var coin3 = Money.FromDecimal(2, "EUR", currencyLookup);

            var banknote = Money.FromDecimal(5, "EUR", currencyLookup);

            Assert.Equal(banknote, coin1 + coin2 + coin3);

        }
        [Fact]
        public void Unused_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(100, "WHAT?", currencyLookup));
        }
        [Fact]
        public void Throw_when_too_many_decimal_places()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Money.FromDecimal(100.123m, "EUR", currencyLookup));
        }
        [Fact]
        public void Throws_on_adding_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = Money.FromDecimal(5, "USD", currencyLookup);

            Assert.Throws<CurrencyMismatchException>(() => firstAmount + secondAmount);
        }
        [Fact]
        public void Throws_on_substracting_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", currencyLookup);
            var secondAmount = Money.FromDecimal(5, "USD", currencyLookup);

            Assert.Throws<CurrencyMismatchException>(() => firstAmount - secondAmount);
        }
    }
}
