using App.Model;
using App.Model.Entity;
using App.Model.RequestModel.Account;
using App.Model.RequestModel.AccountRequestModel;
using App.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Repository.Accounts
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _appDbContext;
        public AccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ServiceResponce<List<AccountResponse>>> AmountTransactionThroughAccountNumbersAsync(AmountTransactionRequest amountTransactionRequest)
        {
           var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                var DebitAccount = await _appDbContext.Accounts.FirstOrDefaultAsync(x => x.IsActive && x.AccountNumber == amountTransactionRequest.SenderAccountNumber);
                var creditAccount = await _appDbContext.Accounts.FirstOrDefaultAsync(x => x.IsActive && x.AccountNumber == amountTransactionRequest.RecieverAccountNumber);

                if(creditAccount is null && DebitAccount is null)
                {
                    throw new Exception(Model.Constants.AppConstants.ACCOUNT_TRANSACTION_FAIL);
                }

                creditAccount.Balance = creditAccount.Balance + amountTransactionRequest.Amount;
                DebitAccount.Balance = DebitAccount.Balance - amountTransactionRequest.Amount;

                if(DebitAccount.Balance < 500) 
                {
                    throw new Exception(Model.Constants.AppConstants.INSUFFICIENT_BALANCE);
                }

                transaction.CommitAsync();

                creditAccount.UpdatedAt = DateTime.Now.ToUniversalTime();
                DebitAccount.UpdatedAt = DateTime.Now.ToUniversalTime();
            
                _appDbContext.Accounts.Update(creditAccount);
                _appDbContext.Accounts.Update(DebitAccount);
                await _appDbContext.SaveChangesAsync();

                List<AccountResponse> responseData = new List<AccountResponse>();

                var creditAccountDetails = await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive && x.Account.AccountNumber == creditAccount.AccountNumber)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        CreatedAt = x.Account.CreatedAt,
                        IsActive = x.Account.IsActive,
                        CustomerName = x.Customer.Name,
                        AccountType = x.Account.AccountTypes.AccountType,
                        Balance = x.Account.Balance,
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                    }).FirstOrDefaultAsync();

                responseData.Add(creditAccountDetails);


                var debitAccountDetails = await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive && x.Account.AccountNumber == DebitAccount.AccountNumber)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        CreatedAt = x.Account.CreatedAt,
                        IsActive = x.Account.IsActive,
                        CustomerName = x.Customer.Name,
                        AccountType = x.Account.AccountTypes.AccountType,
                        Balance = x.Account.Balance,
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                    }).FirstOrDefaultAsync();

                responseData.Add(debitAccountDetails);

                var response = new ServiceResponce<List<AccountResponse>>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_TRANSACTION_SUCCESS,
                    Data = responseData
                };
                return response;



            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var response = new ServiceResponce<List<AccountResponse>>
                {
                    IsSuccess = false,
                    message = ex.Message,
                };
                return response;
            }
        }

        public async Task<ServiceResponce<AccountResponse>> CreateAccountAsync(AccountRequest accountrequest)
        {
            var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var customercheck = await _appDbContext.Customers.FirstOrDefaultAsync(x => x.IsActive && x.Id == accountrequest.CustomerId);

                if (customercheck is null)
                {
                    throw new Exception(Model.Constants.AppConstants.ACCOUNT_CREATE_FAIL);
                }

                Guid guid = Guid.NewGuid();
                Account account = new Account
                {
                    AccountNumber = guid.ToString(),
                    AccountTypeId = accountrequest.AccountTypeId,
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    Balance = accountrequest.Balance,

                };



                _appDbContext.Accounts.Add(account);
                await _appDbContext.SaveChangesAsync();


                AccountCustomerReference accountCustomerReference = new AccountCustomerReference
                {
                    AcccountId = account.Id,
                    CustomerId = accountrequest.CustomerId
                };
                _appDbContext.AccountCustomerReference.Add(accountCustomerReference);
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                var result = await _appDbContext.AccountCustomerReference
                                          .Include(x => x.Account)
                                          .ThenInclude(x => x.AccountTypes)
                                          .Include(x => x.Customer)
                                          .Where(x => x.AcccountId == account.Id)
                                          .Select(x => new AccountResponse
                                          {
                                              Id = x.Account.Id,
                                              AccountNumber = x.Account.AccountNumber,
                                              CreatedAt = x.Account.CreatedAt,
                                              IsActive = x.Account.IsActive,
                                              CustomerName = x.Customer.Name,
                                              AccountType = x.Account.AccountTypes.AccountType,
                                              Balance = x.Account.Balance,
                                              UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                                          }).FirstOrDefaultAsync();



                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_CRAETED,
                    Data = result


                };
                return response;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_CREATE_FAIL
                };
                return response;
            }
        }

        public async Task<ServiceResponce<AccountResponse>> CreditAmountToAccountByAccountNumberAsync(AccountCreditDebitRequest accountCreditRequest)
        {

            try
            {
                var AccountDetails = await _appDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountCreditRequest.AccountNumber && x.IsActive);

                if (AccountDetails is null)
                {
                    throw new Exception(Model.Constants.AppConstants.ACCOUNT_NOT_FOUND);
                }
                AccountDetails.Balance = AccountDetails.Balance + accountCreditRequest.Amount;
                AccountDetails.UpdatedAt = DateTime.Now.ToUniversalTime();

                _appDbContext.Accounts.Update(AccountDetails);
                await _appDbContext.SaveChangesAsync();

                var result =await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive && x.Account.AccountNumber == accountCreditRequest.AccountNumber)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        AccountType = x.Account.AccountTypes.AccountType,
                        CustomerName = x.Customer.Name,
                        Balance = x.Account.Balance,
                        CreatedAt = x.Account.CreatedAt.ToLocalTime(),
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                        IsActive = x.Account.IsActive
                    }).FirstOrDefaultAsync();


                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_TRANSACTION_SUCCESS,
                    Data = result 
                };
                return response;
            }
            catch (Exception ex)
            {

                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return response;
            }
        }

      

        public async Task<ServiceResponce<AccountResponse>> DeactiveAccountByIdAsync(int id)
        {
            try
            {
                var accountdetails = await _appDbContext.Accounts.FirstOrDefaultAsync(z => z.IsActive && z.Id == id);

                if (accountdetails is null)
                {
                    throw new Exception(Model.Constants.AppConstants.ACCOUNT_NOT_FOUND);
                }

                accountdetails.IsActive = false;
                accountdetails.UpdatedAt = DateTime.Now.ToUniversalTime();
                accountdetails.CreatedAt = accountdetails.CreatedAt.ToUniversalTime();

                _appDbContext.Accounts.Update(accountdetails);
                await _appDbContext.SaveChangesAsync();

                var result = await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive && x.Account.Id == id)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        AccountType = x.Account.AccountTypes.AccountType,
                        IsActive = x.Account.IsActive,
                        Balance = x.Account.Balance,
                        CreatedAt = accountdetails.CreatedAt.ToLocalTime(),
                        CustomerName = x.Customer.Name,
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime()
                    }).FirstOrDefaultAsync();

                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_FOUND,
                    Data = result
                };

                return response;

            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = false,
                    message = ex.Message,
                };
                return response;
            }
        }

        public async Task<ServiceResponce<AccountResponse>> DebitAmountFromAccountByAccountNumberAsync(AccountCreditDebitRequest accountDebitRequest)
        {
            var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var accountDetails = await _appDbContext.Accounts.FirstOrDefaultAsync(x => x.IsActive && x.AccountNumber ==  accountDebitRequest.AccountNumber);
                    
                if(accountDetails == null)
                {
                    throw new Exception(Model.Constants.AppConstants.ACCOUNT_NOT_FOUND);
                }

                accountDetails.Balance = accountDetails.Balance - accountDebitRequest.Amount;
                accountDetails.UpdatedAt = DateTime.Now.ToUniversalTime();
                 if (accountDetails.Balance < 500) 
                 { 
                    transaction.RollbackAsync();
                    throw new Exception(Model.Constants.AppConstants.INSUFFICIENT_BALANCE);
                 }

                 _appDbContext.Accounts.Update(accountDetails);
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                var result = await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive && x.Account.AccountNumber == accountDebitRequest.AccountNumber)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        AccountType = x.Account.AccountTypes.AccountType,
                        CustomerName = x.Customer.Name,
                        CreatedAt = x.Account.CreatedAt.ToLocalTime(),
                        Balance = x.Account.Balance,
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                        IsActive = x.Account.IsActive,
                    }).FirstOrDefaultAsync();
                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_TRANSACTION_SUCCESS,
                    Data = result
                };
                return response;
            }catch (Exception ex)
            {
                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = false,
                    message = ex.Message,
                };
                return response;
            }
           
        }

        public async Task<ServiceResponce<AccountResponse>> GetAccountDetailsByIdAsync(int id)
        {
            try
            {

                var result = await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive && x.Account.Id == id)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        AccountType = x.Account.AccountTypes.AccountType,
                        Balance = x.Account.Balance,
                        CreatedAt = x.Account.CreatedAt.ToLocalTime(),
                        CustomerName = x.Customer.Name,
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                        IsActive = x.Account.IsActive
                    }).FirstOrDefaultAsync();

                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_FOUND,
                    Data = result
                };
                return response;

            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<AccountResponse>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return response;
            }
        }

        public async Task<ServiceResponce<List<AccountResponse>>> GetAllAccountAsync()
        {
            try
            {
                var result = await _appDbContext.AccountCustomerReference
                    .Include(x => x.Account)
                    .ThenInclude(x => x.AccountTypes)
                    .Include(x => x.Customer)
                    .Where(x => x.Account.IsActive)
                    .Select(x => new AccountResponse
                    {
                        Id = x.Account.Id,
                        AccountNumber = x.Account.AccountNumber,
                        CustomerName = x.Customer.Name,
                        AccountType = x.Account.AccountTypes.AccountType,
                        CreatedAt = x.Account.CreatedAt.ToLocalTime(),
                        Balance = x.Account.Balance,
                        UpdatedAt = x.Account.UpdatedAt.Value.ToLocalTime(),
                        IsActive = x.Account.IsActive
                    }).ToListAsync();

                var response = new ServiceResponce<List<AccountResponse>>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.ACCOUNT_FOUND,
                    Data = result
                };
                return response;

            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<List<AccountResponse>>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return response;
            }
        }
    }
}
