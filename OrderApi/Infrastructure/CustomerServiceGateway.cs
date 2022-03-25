using OrderApi.Models;
using RestSharp;
using System;

namespace OrderApi.Infrastructure
{
    public class CustomerServiceGateway : IServiceGateway<CustomerDTO>
    {
        string customerServiceBaseUrl;

        public CustomerServiceGateway(string baseUrl)
        {
            customerServiceBaseUrl = baseUrl;
        }
        public CustomerDTO Get(int id)
        {
            RestClient c = new RestClient(customerServiceBaseUrl);

            var request = new RestRequest(id.ToString(), Method.Get);
            var response = c.ExecuteAsync<CustomerDTO>(request);
            response.Wait();
            var customerData = response.Result;
            return customerData.Data;
            
        }
    }
}
