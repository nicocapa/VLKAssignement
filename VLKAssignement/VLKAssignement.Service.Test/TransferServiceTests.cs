using VLKAssignement.DataAccess.Models;
using Xunit;
using FluentAssertions;
using VLKAssignement.Domain;
using VLKAssignement.Service.Interfaces;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using NSubstitute;
using AutoMapper;

namespace VLKAssignement.Service.Test
{
    public class TransferServiceTests
    {
        private ITransferRepository transferRepository;
        private IAccountService accountService;
        private IUserRepository userRepository;
        private IExchangeRateService exchangeRateService;
        private IMapper mapper;

        public TransferServiceTests()
        {
            transferRepository = NSubstitute.Substitute.For<ITransferRepository>();
            accountService = NSubstitute.Substitute.For<IAccountService>();
            userRepository = NSubstitute.Substitute.For<IUserRepository>();
            exchangeRateService = NSubstitute.Substitute.For<IExchangeRateService>();
            mapper = NSubstitute.Substitute.For<IMapper>();
        }

        [Fact]        
        public async Task ShouldUpdateBalanceAndSetStatusSignedToTheTransfer()
        {
            //Arrange
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "USD",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Pending",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "EUR",
                Balance = 15200
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns(transfer);            
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "USD", "EUR").Returns(
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "USD",
                    Rate = 1.18M,
                    RateDate = DateTime.Today,
                    IsBaseCurrencySameAsTo = true
            });

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().BeApproximately(15115.25M, 2);
            transfer.Status.Should().Be(Status.Signed);
        }

        [Fact]
        public async Task ShouldUpdateBalanceAndSetStatusSignedToTheTransferWhenBothCurrenciesAreTheSame()
        {
            //Arrange
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "EUR",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Pending",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "EUR",
                Balance = 15200
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns(transfer);
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "EUR", "EUR").Returns(
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "EUR",
                    Rate = 1.00M,
                    RateDate = DateTime.Today
                });

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().BeApproximately(15100, 0);
            transfer.Status.Should().Be(Status.Signed);
        }

        [Fact]
        public async Task ShouldUpdateBalanceAndSetStatusSignedToTheTransferWhenBothCurrenciesAreDifferentFromEUR()
        {
            //Arrange
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "CHF",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Pending",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "USD",
                Balance = 15200
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns(transfer);
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "CHF", "USD").Returns(
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "CHF",
                    CurrencyCodeTo = "USD",
                    Rate = 1.06M,
                    RateDate = DateTime.Today
                });

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().BeApproximately(15094.00M, 0);
            transfer.Status.Should().Be(Status.Signed);
        }

        [Fact]
        public async Task ShouldCancelTheSigningIfTheTransferIsNotInStatusPending()
        {
            //Arrange            
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "EUR",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Signed",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "EUR",
                Balance = 15200
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns(transfer);
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "EUR", "EUR").Returns(
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "EUR",
                    Rate = 1.00M,
                    RateDate = DateTime.Today
                });

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            var result = await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().Be(15200);
            transfer.Status.Should().Be(Status.Signed);
            result.ValidationResult.Succeded.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldCancelTheSigningIfTheAccountDoesNotHaveEnoughCapital()
        {
            //Arrange
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "EUR",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Pending",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "EUR",
                Balance = 15
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns(transfer);
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "EUR", "EUR").Returns(
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "EUR",
                    Rate = 1.00M,
                    RateDate = DateTime.Today
                });

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            var result = await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().Be(15);
            transfer.Status.Should().Be(Status.Pending);
            result.ValidationResult.Succeded.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldCancelTheSigningIfThereIsNoExchangeRateAvailableForTheSelectedCurrencies()
        {
            //Arrange
            var transfer = new Transfer
            {
                Amount = 1,
                DestinationCurrencyCode = "ARS",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Pending",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "CHF",
                Balance = 15
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns(transfer);
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "ARS", "CHF").Returns((CachedExchangeRate)null);

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            var result = await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().Be(15);
            transfer.Status.Should().Be(Status.Pending);
            result.ValidationResult.Succeded.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldCancelTheSigningIfTheTransferDoesNotExist()
        {
            //Arrange
            var transfer = new Transfer
            {
                Amount = 1,
                DestinationCurrencyCode = "ARS",
                CartId = Guid.NewGuid(),
                DestinationAccountNumber = "TEST",
                Status = "Pending",
                UserId = Guid.NewGuid()
            };
            var account = new Account
            {
                CurrencyCode = "CHF",
                Balance = 15
            };
            var transaction = new Transaction
            {
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                DestinationCurrencyCode = transfer.DestinationCurrencyCode,
                UserId = transfer.UserId
            };

            transferRepository.GetById(Arg.Any<Guid>()).Returns((Transfer)null);
            accountService.GetByUserId(Arg.Any<Guid>()).Returns(account);
            mapper.Map<Transaction>(Arg.Any<Transfer>()).Returns(transaction);

            exchangeRateService.GetExchangeRate(Arg.Any<DateTime>(), "ARS", "CHF").Returns((CachedExchangeRate)null);

            //Act
            var transferService = new TransferService(transferRepository, accountService, mapper, userRepository, exchangeRateService, new TransferValidator(), new TransactionValidator(accountService));
            var result = await transferService.Sign(transfer.Id);

            //Assert
            account.Balance.Should().Be(15);
            transfer.Status.Should().Be(Status.Pending);
            result.ValidationResult.Succeded.Should().BeFalse();
        }

    }
}
