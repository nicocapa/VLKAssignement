using AutoMapper;
using System;
using System.Threading.Tasks;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using VLKAssignement.Domain;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.Service
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IAccountService _accountService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Transfer> _transferValidator;
        private readonly IValidator<Transaction> _transactionValidator;
        private readonly IExchangeRateService _exchangeRateService;


        public TransferService(ITransferRepository transferRepository, IAccountService accountService, IMapper mapper, IUserRepository userRepository, IExchangeRateService exchangeRateService, IValidator<Transfer> transferValidator, IValidator<Transaction> transactionValidator)
        {
            _transferRepository = transferRepository;
            _accountService = accountService;
            _mapper = mapper;
            _userRepository = userRepository;
            _exchangeRateService = exchangeRateService;
            _transferValidator = transferValidator;
            _transactionValidator = transactionValidator;
        }

        public Guid Add(Transfer transfer)
        {            
            var cartId = _userRepository.GetCartId(transfer.UserId);
            transfer.CartId = cartId;
            transfer.Status = Status.Pending;
            transfer = _transferRepository.Add(transfer);
            return transfer.Id;
        }

        public async Task<SignResult> Sign(Guid transferId)
        {
            var signResult = new SignResult();
            
            var transfer = _transferRepository.GetById(transferId);
            if(transfer == null)
            {
                signResult.ValidationResult.Messages.Add("The transfer that you want to sign doesn't exist");
                return signResult;
            }
            var account = _accountService.GetByUserId(transfer.UserId);
            
            var transferValidation = _transferValidator.Validate(transfer);
            if (transferValidation.Succeded)
            {
                var transaction = _mapper.Map<Transaction>(transfer);

                var exchangeRate = await _exchangeRateService.GetExchangeRate(DateTime.Today, transfer.DestinationCurrencyCode, account.CurrencyCode);
                if (exchangeRate == null)
                {
                    signResult.ValidationResult.Messages.Add("There is no exchange rate available for the selected currencies.");
                    return signResult;
                }
                transaction.UsedRate = exchangeRate.Rate;
                transaction.UsedRateDate = exchangeRate.RateDate;
                transaction.IsBaseCurrencySameAsTo = exchangeRate.IsBaseCurrencySameAsTo;

                var transactionValidation = _transactionValidator.Validate(transaction);
                if (transactionValidation.Succeded)
                {
                    transfer.Status = Status.Signed;
                    transfer.UpdatedOn = DateTime.UtcNow;
                    if (exchangeRate.IsBaseCurrencySameAsTo)
                    {
                        account.Balance -= transaction.Amount / transaction.UsedRate;
                    }
                    else
                    {
                        account.Balance -= transaction.Amount * transaction.UsedRate;
                    }

                    signResult.TransactionId = _transferRepository.SignTransfer(transfer, transaction, account);
                }
                else
                {
                    signResult.ValidationResult.Messages.AddRange(transactionValidation.Messages);
                }
            }
            else
            {
                signResult.ValidationResult.Messages.AddRange(transferValidation.Messages);
            }
            return signResult;            
        }
    }
}
