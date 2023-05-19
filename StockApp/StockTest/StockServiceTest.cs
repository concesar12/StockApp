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

        public StockServiceTest(ITestOutputHelper testOutputHelper)
        {
            _stocksService = new StocksService();
            _testOutputHelper = testOutputHelper;
        }

        #region CreateBuyOrder
        [Fact]
        public void Buy_Order_Request_null()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public void Buy_Order_Request_Quantity_Zero(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(100001)]
        public void Buy_Order_Request_Quantity_More_Than_Limit(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        public void Buy_Order_Request_Price_Zero(uint buyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(100001)]
        public void Buy_Order_Request_Price_Over_Limit(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Stock_Symbol_Null()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Stock_Out_Of_Date()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void Buy_Order_Request_Valid_Value()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockName = "Microsoft", Price =  80, Quantity = 3, 
                StockSymbol = "MSFT", DateAndTimeOfOrder = DateTime.Parse("2000-01-01") };

            //Act
            BuyOrderResponse buy_order_response_from_create = _stocksService.CreateBuyOrder(buyOrderRequest);
            List<BuyOrderResponse> buy_order_list = _stocksService.GetBuyOrders();

            //Assert
            Assert.True(buy_order_response_from_create.BuyOrderID != Guid.Empty);
            Assert.Contains(buy_order_response_from_create, buy_order_list);

        }
        #endregion

        #region CreateSellOrder
        [Fact]
        public void Sell_Order_Request_null()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        public void Sell_Order_Request_Quantity_Zero(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(100001)]
        public void Sell_Order_Request_Quantity_More_Than_Limit(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        public void Sell_Order_Request_Price_Zero(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = sellOrderPrice, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(100001)]
        public void Sell_Order_Request_Price_Over_Limit(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Stock_Symbol_Null()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void Sell_Order_Request_Stock_Out_Of_Date()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
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
                DateAndTimeOfOrder = DateTime.Parse("2024-01-01")
            };

            //Act
            SellOrderResponse sell_order_response_from_create = _stocksService.CreateSellOrder(sellOrderRequest);
            List<SellOrderResponse> sell_order_list = _stocksService.GetSellOrders();

            //Assert
            Assert.True(sell_order_response_from_create.SellOrderID != Guid.Empty);
            Assert.Contains(sell_order_response_from_create, sell_order_list);

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
        
        #region GetAllSellOrders

        [Fact]
        public void Get_All_Sell_Orders_Empty_List()
        {
            //Act
            List<SellOrderResponse> sell_orders_from_get = _stocksService.GetSellOrders();

            //Assert
            Assert.Empty(sell_orders_from_get);
        }

        [Fact]
        public void Get_All_Sell_Orders_Add_Few_Orders()
        {
            //Arrange
            SellOrderRequest sell_order_request_1 = new SellOrderRequest()
            {
                StockName = "Microsoft",
                Price = 80,
                Quantity = 3,
                StockSymbol = "MSFT",
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01")
            };

            SellOrderRequest sell_order_request_2 = new SellOrderRequest()
            {
                StockName = "Apple",
                Price = 10,
                Quantity = 2,
                StockSymbol = "APPL",
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01")
            };

            List<SellOrderRequest> sell_orders = new List<SellOrderRequest>() { sell_order_request_1, sell_order_request_2 };
            List<SellOrderResponse> sell_order_responses_list_from_create = new List<SellOrderResponse>();

            foreach (SellOrderRequest orderRequest in sell_orders)
            {
                SellOrderResponse sell_order_response = _stocksService.CreateSellOrder(orderRequest);
                sell_order_responses_list_from_create.Add(sell_order_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (SellOrderResponse sell_order_from_create in sell_order_responses_list_from_create)
            {
                _testOutputHelper.WriteLine(sell_order_from_create.ToString());
            }

            //Act
            List<SellOrderResponse> order_responses_from_get = _stocksService.GetSellOrders();

            _testOutputHelper.WriteLine("Actual");
            foreach (SellOrderResponse sell_order_from_get in order_responses_from_get)
            {
                _testOutputHelper.WriteLine(sell_order_from_get.ToString());
            }

            //Assert
            foreach (SellOrderResponse sell_order_response_from_create in sell_order_responses_list_from_create)
            {
                Assert.Contains(sell_order_response_from_create, order_responses_from_get);
            }

        }
        #endregion
    }
}