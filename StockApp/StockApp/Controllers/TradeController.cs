using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using ServiceContracts;
using ServiceContracts.DTO;

namespace StockApp.Controllers
{
    //The [Route("[controller]")] attribute is used to define the base route for all actions in this controller.
    //Here, [controller] is a placeholder that will be replaced with the name of the controller, which is "Trade" in this case.
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly TradingOptions _tradeOptions;
        private readonly IStocksService _stocksService;


        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="finnhubService"></param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="configuration"></param>
        /// <param name="tradingOptions"></param>
        public TradeController (IFinnhubService finnhubService, IConfiguration configuration, IOptions<TradingOptions> tradingOptions, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _tradeOptions = tradingOptions.Value;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("[action]")] // specifies that the action method can be accessed using a URL that matches the name of the action method. 
        [Route("~/[controller]")] //  specifies that the controller can be accessed using a URL that matches the name of the controller, but with a leading tilde (~) character.
        public IActionResult Index()
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(_tradeOptions.DefaultStockSymbol))
                _tradeOptions.DefaultStockSymbol = "MSFT";

            //We use in this way to be ablo to change the type the stock we want dynamically
            Dictionary<string, object>? responseDictionaryQuote = _finnhubService.GetStockPriceQuote(_tradeOptions.DefaultStockSymbol); // get stock price quotes from API server
            Dictionary<string, object>? responseDictionaryProfile = _finnhubService.GetCompanyProfile(_tradeOptions.DefaultStockSymbol); // get company profile from API server
            StockTrade stock = new StockTrade() { StockSymbol = _tradeOptions.DefaultStockSymbol};

            //Load data from finnHubService into model object
            if (responseDictionaryQuote != null && responseDictionaryProfile != null)
            {
                stock = new StockTrade()
                {
                    StockSymbol = Convert.ToString(responseDictionaryProfile["ticker"]),
                    StockName = Convert.ToString(responseDictionaryProfile["name"]),
                    Quantity = _tradeOptions.DefaultOrderQuantity ?? 0,
                    Price = Convert.ToDouble(responseDictionaryQuote["c"].ToString())
                };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            return View(stock);
        }

        [Route("[action]")]
        public IActionResult Orders()
        {
            //invoke service methods
            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

            //Create model object
            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            //Now add in the viewbag the options read to the view
            ViewBag.TradingOptions = _tradeOptions;
            
            return View(orders);
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
        {
            //update date of order
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear(); // This is usefull to clear up previous scenarios with failures
            TryValidateModel(sellOrderRequest);

            //If there anything wrong
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, StockSymbol = sellOrderRequest.StockSymbol, Quantity = sellOrderRequest.Quantity };
                return View("Index", stockTrade);
            }

            //Invoke service method
            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //Update time of order
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //Re-validate the model
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);
            
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockSymbol = buyOrderRequest.StockSymbol, StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity };
                return View("Index", stockTrade);
            }

            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }
    }
}
