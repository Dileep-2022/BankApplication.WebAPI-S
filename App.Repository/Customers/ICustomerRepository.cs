using App.Model.Entity;
using App.Model.RequestModel.Customer;
using App.Model.RequestModel.CustomerRequestModel;
using App.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repository.Customers
{
    public interface ICustomerRepository
    {
        Task<ServiceResponce<Customer>> AddCustomerAsync(CustomerRequestModel customerRequestModel);

        Task<ServiceResponce<Customer>> GetCustomerByIdAsync(int id);

        Task<ServiceResponce<List<Customer>>> GetAllCustomersAsync();

        Task<ServiceResponce<Customer>> UpdateCustomerAsync(CustomerUpdateRequest customerUpdateRequest);

        Task<ServiceResponce<Customer>> DeactivateCustomerByIdAsync(int id);

    }
}
