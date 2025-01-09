using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class Trade
    {
        public int InsRef {  get; set; }
        public string? TradeReference { get; set; }
        public bool IsTradeBreak { get; set; }
        public string? Price { get; set; }
        public string? Quantity { get; set; }
        public DateTime? Time { get; set; }

        public string Display()
        {
            return string.Format("[{0}] {1} units traded at {2}", Time?.TimeOfDay, Quantity, Price);
        }
    }
}
