using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class OrderEvent
    {
        public enum OrderEventType
        {
            Insert , Update, Delete, Flush
        }

        public int InsRef {  get; set; }
        public OrderEventType OrderType { get; set; }
        public OrderBook.Side Side { get; set; }
        public int Level { get; set; }
        public OrderBookEntry? Entry { get; set; }
        public DateTime Time { get; set; }
    }
}
