using App.Model;
using App.Model.Entity;
using App.Model.RequestModel.AccountType;
using App.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repository.AccountType
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly AppDbContext _AppdbContext;
     public AccountTypeRepository(AppDbContext appdbContext)
        {
            _AppdbContext = appdbContext;
        }

        public async Task<ServiceResponce<AccountTypes>> AddAccountTypeAsync(AccountTypeRequest accountTypeRequest)
        {
            try
            {
                AccountTypes accountTypes = new AccountTypes
                {
                    AccountType = accountTypeRequest.AccountTypes
                };
                _AppdbContext.AccountTypes.Add(accountTypes);
                await _AppdbContext.SaveChangesAsync();

                var result = new ServiceResponce<AccountTypes>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_TYPE_CREATED,
                    Data = accountTypes
                };
                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponce<AccountTypes>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return result;  
            }
        }

        public async Task<ServiceResponce<AccountTypes>> DeleteAccountByIdAsync(int id)
        {
            try
            {
                var result= await GetAccountByIdAsync(id);
                if(result == null)
                {
                    throw new Exception("No Account Found");
                }

                _AppdbContext.AccountTypes.Remove(result.Data);
                await _AppdbContext.SaveChangesAsync();

                var response = new ServiceResponce<AccountTypes>
                { 
                IsSuccess= true,
                message=Model.Constants.AppConstants.ACCOUNT_TYPE_DELETED,
                Data = result.Data
                
                };
                return response;
            }catch (Exception ex)
            {
                var response = new ServiceResponce<AccountTypes>
                {
                    IsSuccess= false,
                    message = ex.Message
                };
                return response;
            }
        }

        public async Task<ServiceResponce<AccountTypes>> GetAccountByIdAsync(int id)
        {
            try
            {
                var result =await  _AppdbContext.AccountTypes.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

                if(result == null)
                {
                    throw new Exception("No AccountType Exists");
                }
                var respose = new ServiceResponce<AccountTypes>
                {
                    IsSuccess= true,
                    message=Model.Constants.AppConstants.ACCOUNT_TYPE_FOUND,
                    Data = result
                };
                return respose;

            }catch (Exception ex)
            {
                var response = new ServiceResponce<AccountTypes>
                {
                    IsSuccess= false,
                    message = ex.Message
                };
                return response;
            }
        }

        public async Task<ServiceResponce<List<AccountTypes>>> GetAllAccountTypesAsync()
        {
            try
            {
                var result = await _AppdbContext.AccountTypes.Where(x => x.IsActive).ToListAsync();

                if (result.Count == 0)
                {
                    throw new Exception("Account Not Found");
                }
                var response = new ServiceResponce<List<AccountTypes>>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_TYPE_FOUND,
                    Data = result
                };
                return response;

            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<List<AccountTypes>>
                {
                    IsSuccess = false,
                    message = ex.Message
                   
                };
                return response;
            }
        }
    }
}
