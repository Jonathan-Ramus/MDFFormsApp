using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class TradeMapper
    {
        public static Trade MapToTrade(string s)
        {
            Trade trade = new();

            string[] lines = s.Trim().Split('\n');
            foreach (string line in lines)
            {
                string[] entry = line.Split(':', 2);
                string field = entry[0].Replace('\"', ' ').Trim();
                string value = entry[1].Replace('\"', ' ').Trim();

                switch (field)
                {
                    case "insref":
                        trade.InsRef = int.Parse(value);
                        break;
                    case "price":
                        trade.Price = value;
                        break;
                    case "quantity":
                        trade.Quantity = value;
                        break;
                    case "time":
                        trade.Time = TimeUtils.Parse(value);
                        break;
                    case "tradereference":
                        trade.TradeReference = value;
                        break;
                    case "tradecode":
                        int code = int.Parse(value);
                        trade.IsTradeBreak = (code & 16) == 16;
                        break;
                }
            }
            return trade;
        }
    }
}
