using Entities;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository :IStocksRepository
    {
        public Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            throw new NotImplementedException();
        }

        public Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            throw new NotImplementedException();
        }

        public Task<List<BuyOrder>> GetBuyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<SellOrder>> GetSellOrders()
        {
            throw new NotImplementedException();
        }
    }
}