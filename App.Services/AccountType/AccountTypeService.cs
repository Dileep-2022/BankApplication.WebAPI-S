using App.Model.Entity;
using App.Model.RequestModel.AccountType;
using App.Model.ResponseModel;
using App.Repository.AccountType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.AccountType
{
    public class AccountTypeService : IAccountTypeServices
    {
        private readonly IAccountTypeRepository _accountTypeRepository;

        public AccountTypeService(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        public async Task<ServiceResponce<AccountTypes>> AddAccountTypeAsync(AccountTypeRequest accountTypeRequest)
        {
            var result = await _accountTypeRepository.AddAccountTypeAsync(accountTypeRequest);
            return result;
        }

        public  async Task<ServiceResponce<AccountTypes>> DeleteAccountByIdAsync(int id)
        {
           var result = await _accountTypeRepository.DeleteAccountByIdAsync(id);
            return result;
        }

        public async Task<ServiceResponce<AccountTypes>> GetAccountByIdAsync(int id)
        {
           var result = await _accountTypeRepository.GetAccountByIdAsync(id);
            return result;
        }

        public async Task<ServiceResponce<List<AccountTypes>>> GetAllAccountTypesAsync()
        {
          var result = await _accountTypeRepository.GetAllAccountTypesAsync();
            return result;
        }
    }
}
