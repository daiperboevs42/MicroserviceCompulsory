using OrderApi.Models;
using RestSharp;
using System;

namespace OrderApi.Infrastructure
{
    public class CustomerServiceGateway : IServiceGateway<CustomerDTO>
    {
        Uri customerServiceBaseUrl;

        public CustomerServiceGateway(Uri baseUrl)
        {
            customerServiceBaseUrl = baseUrl;
        }
        public CustomerDTO Get(int id)
        {
            RestClient c = new RestClient(customerServiceBaseUrl);
            //c.BaseUri = customerServiceBaseUrl;

            var request = new RestRequest(id.ToString(), Method.Get);
            var response = c.ExecuteAsync<CustomerDTO>(request);
            var customerData = response.Result;
            return customerData.Data;
        }
    }
}
