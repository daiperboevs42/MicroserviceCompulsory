using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CustomerAPI.Data
{
    public class CustomerRepo : ICustomerRepo<Customer>
    {
        private readonly CustomerApiContext _context;

        public CustomerRepo(CustomerApiContext context)
        {
            _context = context;
        }
        public Customer CreateCustomer(Customer customerToCreate)
        {
            _context.Attach(customerToCreate).State = EntityState.Added;
            _context.SaveChanges();
            return customerToCreate;
        }

        public Customer DeleteCustomer(int customerID)
        {
            Customer customer = ReadById(customerID);
            _context.Attach(customer).State = EntityState.Deleted;
            _context.SaveChanges();

            return customer;
        }

        public Customer EditCustomer(Customer customerToEdit)
        {
            _context.Attach(customerToEdit).State = EntityState.Modified;
            _context.SaveChanges();
            return customerToEdit;
        }

        public Customer ReadById(int customerID)
        {
            return _context.Customers.AsNoTracking().FirstOrDefault(o => o.customerID == customerID);
        }
    }
}
