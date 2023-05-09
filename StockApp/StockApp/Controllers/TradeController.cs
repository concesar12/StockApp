using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using StockApp.Services;

namespace StockApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public TradeController (FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT"; // Here we are adding the trading options
            }
            //We use in this way to be ablo to change the type the stock we want dynamically
            Dictionary<string, object>? responseDictionaryQuote = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol); // This is to read what has been passed as response
            Dictionary<string, object>? responseDictionaryProfile = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol); // This is to read what has been passed as response
            StockTrade stock = new StockTrade()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                StockName = responseDictionaryProfile["name"].ToString(),
                Price = Convert.ToDouble(responseDictionaryQuote["c"].ToString()),
                Quantity = Convert.ToInt32(responseDictionaryQuote["shareOutstanding"].ToString())
            };

            return View(stock);
        }
    }
}
