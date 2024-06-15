using App.Model.RequestModel.Account;
using App.Model.RequestModel.AccountRequestModel;
using App.Model.ResponseModel;
using App.Repository.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Accounts
{
    public class AccountServices : IAccountServices
    {
        private readonly IAccountRepository _accountRepository;

        public AccountServices(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResponce<List<AccountResponse>>> AmountTransactionThroughAccountNumbersAsync(AmountTransactionRequest amountTransactionRequest)
        {
           var result = await _accountRepository.AmountTransactionThroughAccountNumbersAsync(amountTransactionRequest);
            return result;
        }

        public async Task<ServiceResponce<AccountResponse>> CreateAccountAsync(AccountRequest accountrequest)
        {
            var result = await _accountRepository.CreateAccountAsync(accountrequest);
            return result;
        }

        public async Task<ServiceResponce<AccountResponse>> CreditAmountToAccountByAccountNumberAsync(AccountCreditDebitRequest accountCreditDebitRequest)
        {
            var result = await _accountRepository.CreditAmountToAccountByAccountNumberAsync(accountCreditDebitRequest);
            return result;
        }

        public async Task<ServiceResponce<AccountResponse>> DebitAmountFromAccountByAccountNumberAsync(AccountCreditDebitRequest accountDebitRequest)
        {
            var result = await _accountRepository.DebitAmountFromAccountByAccountNumberAsync(accountDebitRequest);
            return result;
        }

        public async Task<ServiceResponce<AccountResponse>> GetAccountDetailsByIdAsync(int id)
        {
            var result = await _accountRepository.GetAccountDetailsByIdAsync(id);
            return result;
        }

        public async Task<ServiceResponce<List<AccountResponse>>> GetAllAccountAsync()
        {
            var result = await _accountRepository.GetAllAccountAsync();
            return result;
        }

    }
}
