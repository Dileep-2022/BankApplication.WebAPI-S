using App.Model.Entity;
using App.Model.RequestModel.AccountType;
using App.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.AccountType
{
    public interface IAccountTypeServices
    {
        Task<ServiceResponce<AccountTypes>> AddAccountTypeAsync(AccountTypeRequest accountTypeRequest);

        Task<ServiceResponce<AccountTypes>> DeleteAccountByIdAsync(int id);

        Task<ServiceResponce<AccountTypes>> GetAccountByIdAsync(int id);

        Task<ServiceResponce<List<AccountTypes>>> GetAllAccountTypesAsync();
    }
}
