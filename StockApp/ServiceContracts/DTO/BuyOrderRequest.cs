using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest
    {
        [Required(ErrorMessage = "Stock symbol can't be empty")]
        public string? StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name can't be e,pty")]
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(0, 100000, ErrorMessage = "{0} should be between {1} and ${2}")]
        public uint Quantity { get; set; }
        [Range(0, 100000, ErrorMessage = "{0} should be between {1} and ${2}")]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity };
        }
    }
}
