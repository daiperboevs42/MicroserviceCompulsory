using System;
using OrderApi.Models;
using RestSharp;


namespace OrderApi.Infrastructure
{
    public class ProductServiceGateway : IServiceGateway<ProductDTO>
    {
        Uri productServiceBaseUrl;

        public ProductServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }



        public ProductDTO Get(int id)
        {
            RestClient c = new RestClient(productServiceBaseUrl);
            //c.BaseUri = productServiceBaseUrl;

            var request = new RestRequest(id.ToString(), Method.Get);
            var response = c.ExecuteAsync<ProductDTO>(request);
            var orderedProduct = response.Result;
            return orderedProduct.Data;
        }
    }
}
