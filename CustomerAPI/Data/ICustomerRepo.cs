using CustomerAPI.Models;
using System.Collections.Generic;

namespace CustomerAPI.Data
{
    public interface ICustomerRepo<T>
    {
        Customer ReadById(int customerID);
        Customer CreateCustomer(Customer customerToCreate);
        Customer EditCustomer(Customer customerToEdit);
        Customer DeleteCustomer(int customerID);
        IEnumerable<Customer> GetAll();
    }
}
