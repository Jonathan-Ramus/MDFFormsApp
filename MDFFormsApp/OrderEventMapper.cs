using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class OrderEventMapper
    {
        public static OrderEvent MapToOrderEvent(string s)
        {
            OrderEvent orderEvent = new();
            OrderBookEntry orderBookEntry = new();
            DateTime dateTime = DateTime.Today;
            string? dateString = null;
            string? timeString = null;
            bool hasOrderBookEntry = false;

            string[] lines = s.Trim().Split('\n');
            foreach (string line in lines)
            {
                string[] entry = line.Split(':', 2);
                string field = entry[0].Replace('\"', ' ').Trim();
                string value = entry[1].Replace('\"', ' ').Trim();

                switch (field)
                {
                    case "insref":
                        orderEvent.InsRef = int.Parse(value);
                        break;
                    case "mref":
                        switch (value)
                        {
                            case "insertbid":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Insert;
                                orderEvent.Side = OrderBook.Side.BID;
                                hasOrderBookEntry = true;
                                break;
                            case "insertask":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Insert;
                                orderEvent.Side = OrderBook.Side.ASK;
                                hasOrderBookEntry = true;
                                break;
                            case "deletebid":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Delete;
                                orderEvent.Side = OrderBook.Side.BID;
                                break;
                            case "deleteask":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Delete;
                                orderEvent.Side = OrderBook.Side.ASK;
                                break;
                            case "updatebid":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Update;
                                orderEvent.Side = OrderBook.Side.BID;
                                hasOrderBookEntry = true;
                                break;
                            case "updateask":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Update;
                                orderEvent.Side = OrderBook.Side.ASK;
                                hasOrderBookEntry = true;
                                break;
                            case "orderbookflush":
                                orderEvent.OrderType = OrderEvent.OrderEventType.Flush;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "level":
                        orderEvent.Level = int.Parse(value);
                        break;
                    case "price":
                        orderBookEntry.Price = value;
                        break;
                    case "quantity":
                        orderBookEntry.Quantity = value;
                        break;
                    case "numorders":
                        orderBookEntry.NumOrders = value;
                        break;
                    case "date":
                        dateString = value;
                        break;
                    case "time":
                        timeString = value;
                        break;
                }
            }

            if (hasOrderBookEntry)
            {
                orderEvent.Entry = orderBookEntry;
            }

            if (dateString != null)
            {
                string pattern = "yyyy-MM-dd";
                dateTime = DateTime.ParseExact(dateString, pattern, CultureInfo.InvariantCulture);
            }
            if (timeString != null)
            {
                DateTime time = TimeUtils.Parse(timeString);
                dateTime = dateTime.Date + time.TimeOfDay;
            }

            orderEvent.Time = dateTime;

            return orderEvent;
        }
    }
}
