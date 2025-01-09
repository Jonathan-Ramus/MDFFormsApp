using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class OrderBookEntry
    {
        public string? Price { get; set; }
        public string? Quantity { get; set; }
        public string? NumOrders { get; set; }

        public void Update(OrderBookEntry other)
        {
            if (other.Price != null)
            {
                Price = other.Price;
            }
            if (other.Quantity != null)
            {
                Quantity = other.Quantity;
            }
            if (other.NumOrders != null)
            {
                NumOrders = other.NumOrders;
            }
        }
    }
}
