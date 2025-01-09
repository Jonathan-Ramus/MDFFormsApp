using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class OrderBookRepository
    {
        private readonly Dictionary<int, OrderBook> OrderBooks = [];
        public void HandleEvent(OrderEvent orderEvent)
        {
            OrderBook orderBook = OrderBooks.GetValueOrDefault(orderEvent.InsRef, new OrderBook());
            if (!OrderBooks.ContainsKey(orderEvent.InsRef))
            {
                OrderBooks.Add(orderEvent.InsRef, orderBook);
            }
            
            switch (orderEvent.OrderType)
            {
                case OrderEvent.OrderEventType.Insert:
                    orderBook.AddOrderBookEntry(orderEvent.Entry, orderEvent.Level, orderEvent.Side);
                    break;
                case OrderEvent.OrderEventType.Update:
                    orderBook.UpdateOrderBookEntry(orderEvent.Entry, orderEvent.Level, orderEvent.Side);
                    break;
                case OrderEvent.OrderEventType.Delete:
                    orderBook.DeleteOrderBookEntry(orderEvent.Level, orderEvent.Side);
                    break;
                case OrderEvent.OrderEventType.Flush:
                    orderBook.Flush();
                    break;
                default:
                    break;
            }
        }

        public OrderBook? GetOrderBook(int insRef)
        {
            if (!OrderBooks.ContainsKey(insRef))
            {
                return null;
            }
            
           return OrderBooks[insRef];
        }
    }

}
