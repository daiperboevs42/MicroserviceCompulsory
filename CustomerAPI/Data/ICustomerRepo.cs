using CustomerAPI.Models;

namespace CustomerAPI.Data
{
    public interface ICustomerRepo
    {
        Customer ReadById(int customerID);
        Customer CreateCustomer(Customer customerToCreate);
        Customer EditCustomer(Customer customerToEdit);
        Customer DeleteCustomer(int customerID);
    }
}
