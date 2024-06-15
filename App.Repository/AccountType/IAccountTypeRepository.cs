using App.Model.Entity;
using App.Model.RequestModel.AccountType;
using App.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repository.AccountType
{
    public interface IAccountTypeRepository
    {
        Task<ServiceResponce<AccountTypes>> AddAccountTypeAsync(AccountTypeRequest accountTypeRequest);

        Task<ServiceResponce<List<AccountTypes>>> GetAllAccountTypesAsync();

        Task<ServiceResponce<AccountTypes>> DeleteAccountByIdAsync(int id);

        Task<ServiceResponce<AccountTypes>> GetAccountByIdAsync(int id);
    }
}
