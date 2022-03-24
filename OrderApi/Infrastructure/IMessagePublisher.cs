using System.Collections.Generic;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public interface IMessagePublisher
    {
        void PublishCustomerStatusChangedMessage(int customerId,
            decimal totalPrice, string topic);
        void PublishOrderStatusChangedMessage(int? customerId,
            IList<OrderLine> orderLines, string topic);
    }
}
