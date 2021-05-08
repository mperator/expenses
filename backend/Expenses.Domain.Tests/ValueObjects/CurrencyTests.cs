using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.Domain.Tests.ValueObjects
{
    public class CurrencyTests
    {
        [Fact]
        public void CreateCurrency()
        {
            // arrange
            var code = "EUR";

            // act
            var currency = new Currency(code);

            // assert
            Assert.Equal(code, currency.Code); 
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("E")]
        [InlineData("EU")]
        [InlineData("EUR2")]
        public void Currency_WithInvalidCode(string code)
        {
            // act
            var ex = Record.Exception(() => new Currency(code));

            // assert
            Assert.IsType<Exception>(ex);
        }
    }
}
