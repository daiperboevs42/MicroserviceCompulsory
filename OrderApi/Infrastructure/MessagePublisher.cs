using System;
using System.Collections.Generic;
using EasyNetQ;
using OrderApi.Models;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;

        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public void PublishCustomerStatusChangedMessage(int customerId, decimal totalPrice, string topic)
        {
            var message = new CustomerStatusChangedMessage
            {
                CustomerId = customerId,
                TotalPrice = totalPrice
            };

            bus.PubSub.Publish(message, topic);
        }

        public void PublishOrderStatusChangedMessage(int? customerId, IList<OrderLine> orderLines, string topic)
        {
            var message = new SharedModels.OrderStatusChangedMessage
            { 
                CustomerId = customerId,
                OrderLines = orderLines 
            };

            bus.PubSub.Publish(message, topic);
        }

    }
}
