using CustomerAPI.Models;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace CustomerAPI.Data
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the product repository.
        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.PubSub.Subscribe<CustomerStatusChangedMessage>("productApiHkPaid",
                    HandleOrderPaid, x => x.WithTopic("paid"));
                // Add code to subscribe to other OrderStatusChanged events:
                // * cancelled
                // * shipped
                // * paid
                // Implement an event handler for each of these events.
                // Be careful that each subscribe has a unique subscription id
                // (this is the first parameter to the Subscribe method). If they
                // get the same subscription id, they will listen on the same
                // queue.

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }
        private void HandleOrderPaid(CustomerStatusChangedMessage message)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var customerRepo = services.GetService<ICustomerRepo>();
                var cust = customerRepo.ReadById(message.CustomerId);


                cust.CreditStanding = cust.CreditStanding - message.TotalPrice;
                customerRepo.EditCustomer(cust);
            }
        }



    }
}
