using CustomerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerAPI.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { Name = "Tienesh", Email = "tienesh@hotmail.com", Phone = "(800) 444-4444",  ShippingAddress = "SPARTA 1999", BillingAddress = "Esbjerg, Denmark" , CreditStanding = 500 },
                new Customer { Name = "Martin Park", Email = "martinroager@gmail.com", Phone = "(570) 387-0000",  ShippingAddress = "Ponyville 888", BillingAddress = "Esbjerg, Denmark" , CreditStanding = 250 },
                new Customer { Name = "Martin Wøbbe", Email = "martinemilwobbe@gmail.com", Phone = "(845) 354-9912",  ShippingAddress = "Manehatten 2222", BillingAddress = "Esbjerg, Denmark" , CreditStanding = 0 }
            };


            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
