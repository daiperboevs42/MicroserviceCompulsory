using System;
using OrderApi.Models;
using RestSharp;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public class ProductServiceGateway : IServiceGateway<ProductDto>
    {
        string productServiceBaseUrl;

        public ProductServiceGateway(string baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }

        public ProductDto Get(int id)
        {
            RestClient c = new RestClient(productServiceBaseUrl);
           

            var request = new RestRequest(id.ToString(), Method.Get);
            var response = c.ExecuteAsync<ProductDto>(request);
            response.Wait();
            var productData = response.Result;
            return productData.Data;
        }
    }
}
