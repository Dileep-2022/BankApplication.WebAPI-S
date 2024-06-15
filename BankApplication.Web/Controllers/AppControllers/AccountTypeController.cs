using App.Model.RequestModel.AccountType;
using App.Services.AccountType;
using Microsoft.AspNetCore.Mvc;

namespace BankApplication.Web.Controllers.AppControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountTypeController : Controller
    {
        private readonly IAccountTypeServices _accountTypeServices;
        public AccountTypeController(IAccountTypeServices accountTypeServices)
        {
            _accountTypeServices = accountTypeServices;
        }

        [HttpGet("GetAccountTyprById")]
        public async Task<IActionResult> GetAccountTypeByIdAsync(int id)
        {
            var result = await _accountTypeServices.GetAccountByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllAccountTypes")]
        public async Task<IActionResult> GetAllAcountTypesAsync()
        {
            var result = await _accountTypeServices.GetAllAccountTypesAsync();
            return Ok(result);
        }

        [HttpPost("AddAccountType")]
        public async Task<IActionResult> AddAccountTypeAsync(AccountTypeRequest accountTypeRequest)
        {
            var result = await _accountTypeServices.AddAccountTypeAsync(accountTypeRequest);
            return Ok(result);
        }

        [HttpDelete("RemoveAccountTypeById")]
        public async Task<IActionResult> DeleteAccountByIdAsync(int id)
        {
            var result = await _accountTypeServices.DeleteAccountByIdAsync(id);
            return Ok(result);
        }

    }
}
