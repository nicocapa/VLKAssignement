using VLKAssignement.DataAccess.Models;
using Xunit;
using FluentAssertions;
using VLKAssignement.Domain;

namespace VLKAssignement.Service.Test
{
    public class TransferValidatorTests
    {
        [Fact]
        public void ShouldValidateThatTheTransferIsInStatusPendingWhenTryingToSign()
        {
            //Arrange
            var transferValidator = new TransferValidator();
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "EUR",
                Status = Status.Pending
            };

            //Act
            var result = transferValidator.Validate(transfer);

            //Assert
            result.Succeded.Should().BeTrue();
        }

        [Fact]
        public void ShouldValidateThatTheTransferIsNotInStatusPendingWhenTryingToSign()
        {
            //Arrange
            var transferValidator = new TransferValidator();
            var transfer = new Transfer
            {
                Amount = 100,
                DestinationCurrencyCode = "EUR",
                Status = Status.Signed
            };

            //Act
            var result = transferValidator.Validate(transfer);

            //Assert
            result.Succeded.Should().BeFalse();
        }
    }
}
