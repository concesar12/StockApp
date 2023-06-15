using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using ServiceContracts;
using ServiceContracts.DTO;
using Rotativa.AspNetCore;
using StockApp.Filters.ActionFilters;

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

        [Route("[action]/{stockSymbol}")] // specifies that the action method can be accessed using a URL that matches the name of the action method. 
        [Route("~/[controller]/{stockSymbol}")] //  specifies that the controller can be accessed using a URL that matches the name of the controller, but with a leading tilde (~) character.
        public async Task<IActionResult> Index(string stockSymbol)
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = companyProfileDictionary["ticker"].ToString(), StockName = companyProfileDictionary["name"].ToString(), Quantity = _tradeOptions.DefaultOrderQuantity ?? 0, Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            //invoke service methods
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            //Create model object
            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            //Now add in the viewbag the options read to the view
            ViewBag.TradingOptions = _tradeOptions;
            
            return View(orders);
        }

        [Route("[action]")]
        [HttpPost]
        //Filter for model validation
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            //Invoke service method
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        //Filter for model validation
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //Invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        public async Task<IActionResult> TradesPDF()
        {
            //get list of buys and sells orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stocksService.GetBuyOrders());
            orders.AddRange(await _stocksService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradeOptions;

            //Return view as pdf
            return new ViewAsPdf("TradesPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
