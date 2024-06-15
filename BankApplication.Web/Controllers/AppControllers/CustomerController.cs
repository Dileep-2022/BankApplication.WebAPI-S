using App.Model.RequestModel.Customer;
using App.Model.RequestModel.CustomerRequestModel;
using App.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace BankApplication.Web.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomerAsync(CustomerRequestModel customerRequestModel)
        {
            var result=await _customerService.AddCustomerAsync(customerRequestModel);
            return Ok(result);
        }
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var result=await _customerService.GetAllCustomersAsync();
            return Ok(result);
        }

        [HttpGet("GetCustomerById")]
        public async Task<IActionResult> GetCustomerByIdAsync(int Id)
        {
            var result = await _customerService.GetCustomerByIdAsync(Id);
            return Ok(result);
        }
        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomerData(CustomerUpdateRequest customerUpdateRequest)
        {
            var result = await _customerService.UpdateCustomerAsync(customerUpdateRequest);
            return Ok(result);
        }

        [HttpPut("DeactivateCustomer")]
        public async Task<IActionResult> DeactivateCustomerById(int Id)
        {
            var result = await _customerService.DeactivateCustomerByIdAsync(Id);
            return Ok(result);
        }
       
    }
}
