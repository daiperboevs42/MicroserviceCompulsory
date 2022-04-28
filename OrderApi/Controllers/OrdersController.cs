using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Infrastructure;
using OrderApi.Models;
using SharedModels;
using System.Collections.Generic;
using Order = SharedModels.Order;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        IOrderRepository repository;
        IServiceGateway<ProductDto> productServiceGateway;
        IServiceGateway<CustomerDTO> customerServicegateway;
        IMessagePublisher messagePublisher;

        public OrdersController(IRepository<Order> repos,
            IServiceGateway<ProductDto> gateway,
            IServiceGateway<CustomerDTO> Cgateway,
            IMessagePublisher publisher)
        {
            repository = repos as IOrderRepository;
            productServiceGateway = gateway;
            customerServicegateway = Cgateway;
            messagePublisher = publisher;
        }

        // GET orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            if (ProductItemsAvailable(order) != null)
            {
                try
                {
                    if (CreditCheck(order))
                    {
                        // Publish OrderStatusChangedMessage. If this operation
                        // fails, the order will not be created
                        messagePublisher.PublishOrderStatusChangedMessage(
                            order.customerId, order.OrderLines, "completed");

                        // Create order.
                        order.Status = Order.OrderStatus.completed;
                        var newOrder = repository.Add(order);
                        return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);

                    }
                    else
                    {
                        return StatusCode(409, "User's credit is insufficient");
                    }
                }
                catch
                {
                    return StatusCode(500, "An error occurred. Please try again.");
                }
            }
            else
            {
                // If there are not enough product items available.
                return StatusCode(500, "Not enough items in stock.");
            }
        }

        private Order ProductItemsAvailable(Order order)
        {
            decimal totalPrice = 0m;
            foreach (var orderLine in order.OrderLines)
            {
                // Call product service to get the product ordered.
                var orderedProduct = productServiceGateway.Get(orderLine.ProductId);
                totalPrice += orderedProduct.Price;
                if (orderLine.Quantity > orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
                {
                    return null;
                }
            }
            order.TotalPrice = totalPrice;
            return order;
        }

        // PUT orders/5/cancel
        // This action method cancels an order and publishes an OrderStatusChangedMessage
        // with topic set to "cancelled".
        [HttpPut("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            var order = repository.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != Order.OrderStatus.completed)
            {
                return StatusCode(500, "Already shipped");
            }
            try
            {
                messagePublisher.PublishOrderStatusChangedMessage(
                    order.customerId, order.OrderLines, "cancelled");

                order.Status = Order.OrderStatus.cancelled;
                repository.Edit(order);
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500, "An error occurred. Please try again.");
            }
        }

        // PUT orders/5/ship
        // This action method ships an order and publishes an OrderStatusChangedMessage.
        // with topic set to "shipped".
        [HttpPut("{id}/ship")]
        public IActionResult Ship(int id)
        {
            var order = repository.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status == Order.OrderStatus.shipped)
            {
                return StatusCode(500, "Payment already received");
            }
            try
            {
                messagePublisher.PublishOrderStatusChangedMessage(
                    order.customerId, order.OrderLines, "shipped");

                order.Status = Order.OrderStatus.shipped;
                repository.Edit(order);
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500, "An error occurred. Please try again");
            }

        }

        // PUT orders/5/pay
        // This action method marks an order as paid and publishes a CreditStandingChangedMessage
        // (which have not yet been implemented), if the credit standing changes.
        [HttpPut("{id}/pay")]
        public IActionResult Pay(int id)
        {
            var order = repository.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status == Order.OrderStatus.paid)
            {
                return StatusCode(500, "Payment already received");
            }
            try
            {
                messagePublisher.PublishCustomerStatusChangedMessage(
                    order.customerId, order.TotalPrice, "paid");

                order.Status = Order.OrderStatus.paid;
                repository.Edit(order);
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500, "An error occurred. Please try again");
            }
        }
        //Checks if the customers credit score is above zero, and returns true if it is, or false if the credit score is lower than zero
        private bool CreditCheck(Order order)
        {
            var orderCustomer = customerServicegateway.Get(order.customerId);
            
            if (orderCustomer.CreditStanding <= 0)
            {
                return false;
            }
            return true;
        }

    }
}
