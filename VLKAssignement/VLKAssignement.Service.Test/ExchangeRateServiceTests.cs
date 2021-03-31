using VLKAssignement.DataAccess.Models;
using Xunit;
using FluentAssertions;
using VLKAssignement.Domain;
using VLKAssignement.Service.Interfaces;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using NSubstitute;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace VLKAssignement.Service.Test
{
    public class ExchangeRateServiceTests
    {
        [Fact]
        public async Task ShouldReturnARateEqualsTo1WhenBothCurrenciesAreTheSame()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act

            var result = await exchangeRateService.GetExchangeRate(DateTime.Today, "EUR", "EUR");

            //Assert
            result.Rate.Should().Be(1);
        }

        [Fact]
        public async Task ShouldReturnAnEmptyResultWhenThereIsNoDataAvailable()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "EUR", "USD").Returns(new System.Collections.Generic.List<CachedExchangeRate>());

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act

            var result = await exchangeRateService.GetExchangeRate(DateTime.Today, "EUR", "USD");

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnARateAndIndicateThatItShouldMultiplyTheAmountWhenConvertingToAccountCurrency()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "EUR", "USD").Returns(new System.Collections.Generic.List<CachedExchangeRate>
            {
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "USD",
                    Rate = 1.18M,
                    RateDate = DateTime.Today
                }
            });

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act

            var result = await exchangeRateService.GetExchangeRate(DateTime.Today, "EUR", "USD");

            //Assert
            result.Should().NotBeNull();
            result.Rate.Should().Be(1.18M);
            result.IsBaseCurrencySameAsTo.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnARateAndIndicateThatItShouldDivideTheAmountWhenConvertingToAccountCurrency()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "USD", "EUR").Returns(new System.Collections.Generic.List<CachedExchangeRate>
            {
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "USD",
                    Rate = 1.18M,
                    RateDate = DateTime.Today
                }
            });

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act
            var result = await exchangeRateService.GetExchangeRate(DateTime.Today, "USD", "EUR");

            //Assert
            result.Should().NotBeNull();
            result.Rate.Should().Be(1.18M);
            result.IsBaseCurrencySameAsTo.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnACalculatedRateBetweenFromAndToCurrencyAndIndicateThatItShouldMultiplyTheAmountWhenConvertingToAccountCurrency()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "GBP", "USD").Returns(new System.Collections.Generic.List<CachedExchangeRate>
            {
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "USD",
                    Rate = 1.18M,
                    RateDate = DateTime.Today
                },
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "GBP",
                    Rate = 0.85M,
                    RateDate = DateTime.Today
                }
            });

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act
            var result = await exchangeRateService.GetExchangeRate(DateTime.Today, "GBP", "USD");

            //Assert
            result.Should().NotBeNull();
            result.Rate.Should().BeApproximately(1.38M,2);
            result.IsBaseCurrencySameAsTo.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnAnEmptyResultWhenThereIsNoDataAvailableInTheDatabaseAndInTheVendorAPI()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "EUR", "USD").Returns(new System.Collections.Generic.List<CachedExchangeRate>());
            wrapperExchangeRateAPI.GetLatestRates().Returns(Task.FromResult((ExchangeRateResult)null));


            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act
            var result = await exchangeRateService.GetExchangeRate(DateTime.Today, "EUR", "USD");

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotCallTheVendorAPIWhenThereIsDataInTheDatabase()
        {
            //Arrange
            var wrapperExchangeRateAPI = NSubstitute.Substitute.For<IWrapperExchangeRateAPI>();
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();

            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "USD", "EUR").Returns(new System.Collections.Generic.List<CachedExchangeRate>
            {
                new CachedExchangeRate
                {
                    CurrencyCodeFrom = "EUR",
                    CurrencyCodeTo = "USD",
                    Rate = 1.18M,
                    RateDate = DateTime.Today
                }
            });

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);

            //Act
            await exchangeRateService.GetExchangeRate(DateTime.Today, "USD", "EUR");

            //Assert
            await wrapperExchangeRateAPI.DidNotReceive().GetLatestRates();
        }

        [Fact]
        public async Task ShouldReturnEmptyResultInCaseTheVendorApiFails()
        {
            //Arrange
            var cachedExchangeRateRepository = NSubstitute.Substitute.For<ICachedExchangeRateRepository>();
            var httpClientFactory = NSubstitute.Substitute.For<IHttpClientFactory>();
            var logger = NSubstitute.Substitute.For<ILogger<WrapperExchangeRateAPI>>();

            var wrapperExchangeRateAPI = new WrapperExchangeRateAPI(httpClientFactory, logger);
            cachedExchangeRateRepository.GetRate(Arg.Any<DateTime>(), "USD", "EUR").Returns(new System.Collections.Generic.List<CachedExchangeRate>());

            var exchangeRateService = new ExchangeRateService(wrapperExchangeRateAPI, cachedExchangeRateRepository);
            
            //Act
            var result =await exchangeRateService.GetExchangeRate(DateTime.Today, "USD", "EUR");

            //Assert
            result.Should().BeNull();
        }
    }
}
