using CustomerAPI.Data;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _custRepo;

        public CustomerController(ICustomerRepo repos)
        {
            _custRepo = repos;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_custRepo.ReadById(id));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                return Ok(_custRepo.CreateCustomer(customer));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            try
            {
                if (id != customer.customerID)
                {
                    return BadRequest("Parameter ID and owner ID have to be the same");
                }

                return Ok(_custRepo.EditCustomer(customer));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_custRepo.DeleteCustomer(id));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
