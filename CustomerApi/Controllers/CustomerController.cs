using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CustomerApi.Models;
using CustomerApi.Services;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerStore _store;

        public CustomerController(ICustomerStore store, ILogger<CustomerController> logger)
        {
            _logger = logger;
            _store = store;
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] List<Customer> customers)
        {
            var validCustomers = new List<Customer>();

            var allErrors = new List<string>();

            foreach (var customer in customers)
            {
                if (_store.Exists(customer.Id))
                {
                    allErrors.Add($"Customer ID {customer.Id} already exists.");
                    continue;
                }

                validCustomers.Add(customer);
            }

            if (allErrors.Any())
            {
                return BadRequest(new { Errors = allErrors });
            }
            
            _store.AddCustomers(validCustomers);
            return Ok();
        }


        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(_store.GetAll());
        }
    }
}
