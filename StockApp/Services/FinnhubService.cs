using ServiceContracts;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json; //for API response
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        //Add finnhub repository
        private readonly IFinnhubRepository _finnhubRepository;
        //Adding logger in here
        private readonly ILogger<FinnhubService> _logger;
        //Contructor finnhub repository
        public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
        {
            _logger = logger;
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            //invoke repository
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);

            //return response dictionary back to the caller
            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol) // This is the task that makes the request
        {
            //invoke repository
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            //return response dictionary back to the caller
            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            _logger.LogInformation("This is entering the getstocks method in finhub service");
            List<Dictionary<string, string>>? responseListStocks = await _finnhubRepository.GetStocks();
            return responseListStocks;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            //invoke repository
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
            //return response dictionary back to the caller\
            return responseDictionary;
        }
    }
}
