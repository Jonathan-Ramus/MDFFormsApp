using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class TradeRepository
    {
        private readonly Dictionary<int, List<Trade>> TradeLists = [];

        public void Add(Trade t)
        {
            if (!TradeLists.ContainsKey(t.InsRef))
            {
                TradeLists.Add(t.InsRef, new List<Trade>());
            }

            List<Trade> trades = TradeLists[t.InsRef];

            if (t.IsTradeBreak)
            {
                Trade? affectedTrade = trades.Where(x => x.TradeReference == t.TradeReference).FirstOrDefault();
                if (affectedTrade != null)
                {
                    affectedTrade.IsTradeBreak = true;
                } else
                {
                    trades.Add(t);
                }
            }
            else if (trades.Where(x => x.TradeReference == t.TradeReference).FirstOrDefault() == null)
            {
                trades.Add(t);
            }
        }

        public List<Trade> GetFromTime(int insRef, DateTime? time)
        {
            if (!TradeLists.ContainsKey(insRef))
            {
                return [];
            }

            List<Trade> result = [];
            List<Trade> trades = TradeLists[insRef];
            foreach (Trade t in trades)
            {
                if (t.Time > time && !t.IsTradeBreak)
                {
                    result.Add(t);
                }
            }
            return result;
        }
    }
}
