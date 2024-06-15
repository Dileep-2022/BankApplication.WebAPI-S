using App.Model.RequestModel.Account;
using App.Model.RequestModel.AccountRequestModel;
using App.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Accounts
{
    public interface IAccountServices
    {
        Task<ServiceResponce<AccountResponse>> CreateAccountAsync(AccountRequest accountrequest);
        Task<ServiceResponce<AccountResponse>> GetAccountDetailsByIdAsync(int id);
        Task<ServiceResponce<List<AccountResponse>>> GetAllAccountAsync();
        Task<ServiceResponce<AccountResponse>> CreditAmountToAccountByAccountNumberAsync(AccountCreditDebitRequest accountCreditDebitRequest);
        Task<ServiceResponce<AccountResponse>> DebitAmountFromAccountByAccountNumberAsync(AccountCreditDebitRequest accountDebitRequest);
        Task<ServiceResponce<List<AccountResponse>>> AmountTransactionThroughAccountNumbersAsync(AmountTransactionRequest amountTransactionRequest);

    }
}
