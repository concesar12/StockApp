using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repositories
{
    public class FinnhubRepository :IFinnhubRepository
    {
        //Adding configuration from appsettings.json
        private readonly IConfiguration _configuration;

        //Adding the http client to call the API in Finnhub
        private readonly IHttpClientFactory _httpClientFactory;

        //Creating constructor to initialize properties
        public FinnhubRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            //Create http client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //Create http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                //URI includes the secret token
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            //Send request To Finnhub API
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync( httpRequestMessage );

            //read response body
            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

            //Convert response body (from JSON into dictionary)
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>( responseBody );

            //Validation of data
            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");
            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            //return response dictionary back to the caller
            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            throw new NotImplementedException();
        }
    }
}
