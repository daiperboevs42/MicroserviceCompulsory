namespace OrderApi.Models
{
    public class CustomerStatusChangedMessage
    {
        public int CustomerId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
