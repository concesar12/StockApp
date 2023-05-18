using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;
using Xunit.Abstractions;

namespace StockTest
{
    public class StockServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _testOutputHelper;

        public StockServiceTest()
        {
            _stocksService = new StocksService();
        }

        #region CreateBuyOrder
        [Fact]
        public void Buy_Order_Request_null()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act
            Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Quantity_Zero()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() {Quantity = 0 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Quantity_More_Than_Limit()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { Quantity = 100001 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Price_Zero()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { Price = 0 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Price_Over_Limit()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { Price = 100001 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Stock_Symbol_Null()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = null };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Stock_Out_Of_Date()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { DateAndTimeOfOrder = DateTime.Parse("2000-01-01") };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Valid_Value()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockName = "Microsoft", Price =  80, Quantity = 3, 
                StockSymbol = "MSFT", DateAndTimeOfOrder = DateTime.Parse("2000-01-01") };

            //Act
            Task<BuyOrderResponse> buy_order_response_from_create = _stocksService.CreateBuyOrder(buyOrderRequest);
            Task<List<BuyOrderResponse>?> buy_order_list = _stocksService.GetBuyOrders();

            //Assert
            Assert.True(buy_order_response_from_create.Result.BuyOrderID != Guid.Empty);
            Assert.Contains(buy_order_response_from_create.Result, buy_order_list?.Result);

            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }
        #endregion

        #region CreateSellOrder
        [Fact]
        public void Sell_Order_Request_null()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Quantity_Zero()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { Quantity = 0 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Quantity_More_Than_Limit()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { Quantity = 100001 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Price_Zero()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { Price = 0 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Price_Over_Limit()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { Price = 100001 };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Stock_Symbol_Null()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = null };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Stock_Out_Of_Date()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { DateAndTimeOfOrder = DateTime.Parse("2000-01-01") };

            //Act
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Valid_Value()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockName = "Microsoft",
                Price = 80,
                Quantity = 3,
                StockSymbol = "MSFT",
                DateAndTimeOfOrder = DateTime.Parse("2000-01-01")
            };

            //Act
            Task<SellOrderResponse> sell_order_response_from_create = _stocksService.CreateSellOrder(sellOrderRequest);
            Task<List<SellOrderResponse>?> sell_order_list = _stocksService.GetSellOrders();

            //Assert
            Assert.True(sell_order_response_from_create.Result.SellOrderID != Guid.Empty);
            Assert.Contains(sell_order_response_from_create.Result, sell_order_list?.Result);

            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }
        #endregion

        #region GetAllBuyOrders
        [Fact]
        public void Get_All_Buy_Orders_Empty_List()
        {
            //Act
            List<BuyOrderResponse> buy_orders_from_get = _stocksService.GetBuyOrders();

            //Assert
            Assert.Empty(buy_orders_from_get);
        }

        [Fact]
        public void Get_All_Buy_Orders_Add_Few_Orders()
        {
            //Arrange
            BuyOrderRequest buy_order_request_1 = new BuyOrderRequest()
            {
                StockName = "Microsoft",
                Price = 80,
                Quantity = 3,
                StockSymbol = "MSFT",
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01")
            };

            BuyOrderRequest buy_order_request_2 = new BuyOrderRequest()
            {
                StockName = "Apple",
                Price = 10,
                Quantity = 2,
                StockSymbol = "APPL",
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01")
            };

            List<BuyOrderRequest> buy_orders = new List<BuyOrderRequest>() { buy_order_request_1, buy_order_request_2 };
            List<BuyOrderResponse> buy_order_responses_list_from_create = new List<BuyOrderResponse>();

            foreach (BuyOrderRequest orderRequest in buy_orders)
            {
                BuyOrderResponse buy_order_response = _stocksService.CreateBuyOrder(orderRequest);
                buy_order_responses_list_from_create.Add(buy_order_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (BuyOrderResponse buy_order_from_create in buy_order_responses_list_from_create)
            {
                _testOutputHelper.WriteLine(buy_order_from_create.ToString());
            }

            //Act
            List<BuyOrderResponse> order_responses_from_get = _stocksService.GetBuyOrders();

            _testOutputHelper.WriteLine("Actual");
            foreach (BuyOrderResponse buy_order_from_get in order_responses_from_get)
            {
                _testOutputHelper.WriteLine(buy_order_from_get.ToString());
            }

            //Assert
            foreach (BuyOrderResponse buy_order_response_from_create in buy_order_responses_list_from_create)
            {
                Assert.Contains(buy_order_response_from_create, order_responses_from_get);
            }

        }
        #endregion
    }
}