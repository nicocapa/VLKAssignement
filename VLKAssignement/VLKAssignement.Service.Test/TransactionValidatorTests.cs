using NSubstitute;
using System;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.Service.Interfaces;
using Xunit;
using FluentAssertions;
namespace VLKAssignement.Service.Test
{
    public class TransactionValidatorTests
    {
        [Fact]
        public void ShouldValidateThatTheAccountDoesNotHaveEnoughMoney()
        {
            //Arrange
            var accountService = GivenAnAccount("EUR", 0);

            var transactionValidator = new TransactionValidator(accountService);
            var transaction = new Transaction
            {
                Amount = 100,
                UsedRate = 1,
                DestinationCurrencyCode = "EUR"
            };

            //Act
            var result = transactionValidator.Validate(transaction);

            //Assert
            result.Succeded.Should().BeFalse();
        }

        [Fact]
        public void ShouldValidateThatTheAccountDoesHaveEnoughMoney()
        {
            //Arrange
            var accountService = GivenAnAccount("EUR", 101M);

            var transactionValidator = new TransactionValidator(accountService);
            var transaction = new Transaction
            {
                Amount = 100,
                UsedRate = 1,
                DestinationCurrencyCode = "EUR"
            };

            //Act
            var result = transactionValidator.Validate(transaction);

            //Assert
            result.Succeded.Should().BeTrue();
        }

        [Fact]
        public void ShouldValidateThatTheAccountDoesHaveEnoughMoneyTakingIntoAccountTheRateExchange()
        {
            //Arrange
            var accountService = GivenAnAccount("USD", 101M);
            
            var transactionValidator = new TransactionValidator(accountService);
            var transaction = new Transaction
            {
                Amount = 100,
                UsedRate = 1.18M,
                DestinationCurrencyCode = "EUR",
                IsBaseCurrencySameAsTo = true
            };

            //Act
            var result = transactionValidator.Validate(transaction);

            //Assert
            result.Succeded.Should().BeTrue();
        }

        private IAccountService GivenAnAccount(string currencyCode, decimal balance)
        {
            var accountService = NSubstitute.Substitute.For<IAccountService>();
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(new DataAccess.Models.Account
            {
                UserId = Guid.NewGuid(),
                Balance = balance,
                CurrencyCode = currencyCode,
                IBAN = "TestIBAN"
            });

            return accountService;
        }
    }
}
