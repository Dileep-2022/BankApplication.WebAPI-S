using App.Model;
using App.Model.Entity;
using App.Model.RequestModel.Customer;
using App.Model.RequestModel.CustomerRequestModel;
using App.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Repository.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _AppDbcontext;
        public CustomerRepository(AppDbContext dbContext)
        {
            _AppDbcontext = dbContext;
        }
        public async Task<ServiceResponce<Customer>> AddCustomerAsync(CustomerRequestModel customerRequestModel)
        {

            try
            {

                Guid guid = Guid.NewGuid();
                Customer customer = new Customer
                {
                    CustomerId = guid.ToString(),
                    Name = customerRequestModel.Name,
                    PanNumber = customerRequestModel.PanNumber,
                    Dateofbirth = customerRequestModel.Dateofbirth,
                    Address = customerRequestModel.Address,
                    CreatedAt = DateTime.Now.ToUniversalTime(),

                };
                _AppDbcontext.Customers.Add(customer);
                await _AppDbcontext.SaveChangesAsync();

                customer.CreatedAt = customer.CreatedAt.ToLocalTime();
                customer.Dateofbirth = customer.Dateofbirth.Date;

                var result = new ServiceResponce<Customer>();
                result.IsSuccess = true;
                result.message = Model.Constants.AppConstants.CUSTOMER_CREATED;
                result.Data = customer;

                return result;

            }
            catch (Exception ex)
            {
                var result = new ServiceResponce<Customer>();
                result.IsSuccess = false;
                result.message = ex.Message;

                return result;
            }
        }
        public async Task<ServiceResponce<Customer>> GetCustomerByIdAsync(int Id)
        {
            try
            {
                var result = await _AppDbcontext.Customers.FirstOrDefaultAsync(x => x.Id == Id && x.IsActive);
                if (result is null)
                {
                    throw new Exception(Model.Constants.AppConstants.CUSTOMER_NOT_FOUND);
                }

                result.CreatedAt = result.CreatedAt.ToLocalTime();
                result.UpdatedAt = result.UpdatedAt?.ToLocalTime();
                result.Dateofbirth = result.Dateofbirth.Date;

                var response = new ServiceResponce<Customer>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.CUSTOMER_FOUND,
                    Data = result
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<Customer>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return response;
            }
        }

        public async Task<ServiceResponce<List<Customer>>> GetAllCustomersAsync()
        {
            try
            {
                List<Customer> allCustomers = await _AppDbcontext.Customers.Where(x => x.IsActive).ToListAsync();
                allCustomers.ForEach(x =>
                {
                    x.CreatedAt = x.CreatedAt.ToLocalTime();
                    x.UpdatedAt = x.UpdatedAt?.ToLocalTime();
                    x.Dateofbirth= x.Dateofbirth.Date;
                });


                if (!allCustomers.Any())
                {
                    throw new Exception(Model.Constants.AppConstants.NO_CUSTOMER_EXISTS);
                }
                var response = new ServiceResponce<List<Customer>>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.CUSTOMER_FOUND,
                    Data = allCustomers
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<List<Customer>>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return response;
            }
        }

        public async Task<ServiceResponce<Customer>> UpdateCustomerAsync(CustomerUpdateRequest customerUpdateRequest)
        {
            try
            {
                var result = await GetCustomerByIdAsync(customerUpdateRequest.Id);
                if (result is null)
                {
                    throw new Exception(Model.Constants.AppConstants.CUSTOMER_NOT_FOUND);
                }
                result.Data.Name = customerUpdateRequest.Name;
                result.Data.Address = customerUpdateRequest.Address;
                result.Data.PanNumber = customerUpdateRequest.PanNumber;
                result.Data.Dateofbirth = customerUpdateRequest.Dateofbirth;
                result.Data.UpdatedAt = DateTime.Now.ToUniversalTime();
                result.Data.CreatedAt = result.Data.CreatedAt.ToUniversalTime();



                _AppDbcontext.Customers.Update(result.Data);
                await _AppDbcontext.SaveChangesAsync();

                result.Data.UpdatedAt = result.Data.UpdatedAt?.ToLocalTime();
                result.Data.CreatedAt = result.Data.CreatedAt.ToLocalTime();

                var response = new ServiceResponce<Customer>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.CUSTOMER_DATA_UPDATED,
                    Data = result.Data
                };
                return response;

            }
            catch (Exception ex)
            {
                var response = new ServiceResponce<Customer>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return response;
            }
        }

        public async Task<ServiceResponce<Customer>> DeactivateCustomerByIdAsync(int id)
        {
            try
            {
                var result = await GetCustomerByIdAsync(id);
                if(!result.IsSuccess)
                {
                    throw new Exception(Model.Constants.AppConstants.CUSTOMER_NOT_FOUND);
                }

                result.Data.IsActive = false;
                result.Data.UpdatedAt = DateTime.Now.ToUniversalTime();
                result.Data.CreatedAt = result.Data.CreatedAt.ToUniversalTime();

                _AppDbcontext.Customers.Update(result.Data);
                await _AppDbcontext.SaveChangesAsync();


                result.Data.UpdatedAt = result.Data.UpdatedAt?.ToLocalTime();
                result.Data.CreatedAt = result.Data.CreatedAt.ToLocalTime();
                var response = new ServiceResponce<Customer>
                {
                    IsSuccess = true,
                    message = Model.Constants.AppConstants.CUSTOMER_DEACTIVATION_SUCCESS,
                    Data = result.Data
                };

                return response;

            }catch(Exception ex)
            {
                var result = new ServiceResponce<Customer>
                {
                    IsSuccess = false,
                    message = ex.Message
                };
                return result;
            }
        }
    }
}
