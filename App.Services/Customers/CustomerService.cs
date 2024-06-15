using App.Model.Entity;
using App.Model.RequestModel.Customer;
using App.Model.RequestModel.CustomerRequestModel;
using App.Model.ResponseModel;
using App.Repository.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository) {
            _customerRepository = customerRepository;        
        }
        public async Task<ServiceResponce<Customer>> AddCustomerAsync(CustomerRequestModel customerRequestModel)
        {
          var result=await  _customerRepository.AddCustomerAsync(customerRequestModel);
            return result;
        }

        public async Task<ServiceResponce<List<Customer>>> GetAllCustomersAsync()
        {
            var result=await _customerRepository.GetAllCustomersAsync();
            return result;
        }

        public async Task<ServiceResponce<Customer>> GetCustomerByIdAsync(int id)
        {
           var result=await _customerRepository.GetCustomerByIdAsync(id);
            return result;
        }
     
        public async Task<ServiceResponce<Customer>> UpdateCustomerAsync(CustomerUpdateRequest customerUpdateRequest)
        {
            var result = await _customerRepository.UpdateCustomerAsync(customerUpdateRequest);
            return result;
        }
        public async Task<ServiceResponce<Customer>> DeactivateCustomerByIdAsync(int id)
        {
            var result = await _customerRepository.DeactivateCustomerByIdAsync(id);
            return result;
        }
    }
}
