using ServiceContracts;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json; //for API response

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Dictionary<string, object>? GetCompanyProfile(string stockSymbol)
        {
            //by using using, we ensure that the HttpClient instance is disposed of correctly and efficiently,
            //and that any unmanaged resources it has created are released. This helps to prevent potential issues such as memory leaks or socket exhaustion.
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                //Validation
                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from finnhub server");
                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                return responseDictionary;

            }
        }

        public Dictionary<string, object>? GetStockPriceQuote(string stockSymbol) // This is the task that makes the request
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage); //This is to receive the message

                Stream stream = httpResponseMessage.Content.ReadAsStream(); // This is to create the stream to read it

                StreamReader streamReader = new StreamReader(stream); //This is to actual read the message from response body

                string response = streamReader.ReadToEnd(); //We read into a string the stream
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response); // We are converting what we get in response into a dictionary 
                if (responseDictionary == null) // In case it fails
                    throw new InvalidOperationException("No response from finnhub server");

                if (responseDictionary.ContainsKey("error")) // in case there is an error
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                return responseDictionary;
            }
        }
    }
}
