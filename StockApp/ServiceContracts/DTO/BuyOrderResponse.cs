using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }


        public override string ToString()
        {
            return $"Buy order Id: {BuyOrderID}, Stock Symbol: {StockSymbol}, Stock name: {StockName}, Date and time of order: {DateAndTimeOfOrder?.ToString("dd MM yyyy")}, Quantity:{Quantity}, Price: ${Price}, Trade Amount: {TradeAmount}";
        }
    }
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
            };
        }
    }

    
}
