using System.Collections.Generic;
using static OrderApi.Models.Order;

namespace OrderApi.Models
{
    public class OrderStatusChangedMessage
    {
        public int CustomerId { get; set; }
        public IList<OrderLine> OrderLines { get; set; }
    }
}
