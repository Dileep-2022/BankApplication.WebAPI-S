using App.Model.RequestModel.Account;
using App.Model.RequestModel.AccountRequestModel;
using App.Services.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BankApplication.Web.Controllers.AppControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices;

        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccountAsync(AccountRequest accountRequest)
        {
            var result = await _accountServices.CreateAccountAsync(accountRequest);
            return Ok(result);
        }

        [HttpGet("GetAccountByID")]
        public async Task<IActionResult> GetAccountByIdAsync(int id)
        {
            var result = await _accountServices.GetAccountDetailsByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var result = await _accountServices.GetAllAccountAsync();
            return Ok(result);
        }

        [HttpPost("CreditAmountToAccount")]
        public async Task<IActionResult> CreditAmountToAccountByAccountNumberAsync(AccountCreditDebitRequest accountCreditRequest)
        {
           var result = await _accountServices.CreditAmountToAccountByAccountNumberAsync(accountCreditRequest);
            return Ok(result);
        }

        [HttpPost("DebitAmountFromAccount")]
        public async Task<IActionResult> DebitAmountFromAccountByAccountNumberAsync(AccountCreditDebitRequest debitRequest)
        {
            var result = await _accountServices.DebitAmountFromAccountByAccountNumberAsync(debitRequest);
            return Ok(result);
        }

        [HttpPost("AmountTransactionThroghAccountnumber")]
        public async Task<IActionResult> AmountTransactionThroughAccountNumbersAsync(AmountTransactionRequest amountTransactionRequest)
        {
            var result = await _accountServices.AmountTransactionThroughAccountNumbersAsync(amountTransactionRequest);
            return Ok(result);
        }
    }
}
