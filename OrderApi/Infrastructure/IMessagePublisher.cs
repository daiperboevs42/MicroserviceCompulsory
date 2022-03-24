using System.Collections.Generic;
using static OrderApi.Models.Order;

namespace OrderApi.Infrastructure
{
    public interface IMessagePublisher
    {
        void PublishOrderStatusChangedMessage(int customerId,
            IList<OrderLine> orderLines, string topic);
        void PublishCustomerStatusChangedMessage(int customerId,
           decimal totalPrice, string topic);
    }
}
