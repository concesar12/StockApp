﻿using Entities;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public override string ToString()
        {
            return $"Sell order Id: {SellOrderID}, Stock Symbol: {StockSymbol}, Stock name: {StockName}, Date and time of order: {DateAndTimeOfOrder?.ToString("dd MM yyyy")}, Quantity:{Quantity}, Price: ${Price}, Trade Amount: {TradeAmount}";
        }
    }

    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
            };
        }
    }
}
