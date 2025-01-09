using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class OrderBook
    {
        public enum Side
        {
            BID, ASK
        }
        
        private readonly List<OrderBookEntry> Bids = [];
        private readonly List<OrderBookEntry> Asks = [];

        public void AddOrderBookEntry(OrderBookEntry entry, int level, Side side)
        {
            List<OrderBookEntry> orderBookSide = side == Side.BID ? Bids : Asks;
            orderBookSide.Insert(level-1, entry);
        }

        public void DeleteOrderBookEntry(int level, Side side)
        {
            List<OrderBookEntry> orderBookSide = side == Side.BID ? Bids : Asks;
            orderBookSide.RemoveAt(level-1);
        }

        public void UpdateOrderBookEntry(OrderBookEntry entry, int level, Side side)
        {
            List<OrderBookEntry> orderBookSide = side == Side.BID ? Bids : Asks;
            orderBookSide[level-1].Update(entry);
        }

        public void Flush()
        {
            Bids.Clear();
            Asks.Clear();
        }

        public List<OrderBookEntry> GetBids()
        {
            return [.. Bids];
        }

        public List<OrderBookEntry> GetAsks()
        { 
            return [.. Asks];
        }
    }
}
