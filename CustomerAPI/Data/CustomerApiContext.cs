using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace CustomerAPI.Data
{
    public class CustomerApiContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public CustomerApiContext(DbContextOptions<CustomerApiContext> options)
           : base(options)
        {
        }

        public System.Data.Entity.DbSet<Customer> Customers { get; set; }
    }
}
