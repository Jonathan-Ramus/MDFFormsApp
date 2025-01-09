namespace MDFFormsApp;

using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

class Program
{
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();

    const string MC_TRADE = "4";
    const string MC_ORDER = "8";
    const string MC_ORDER_BOOK_FLUSH = "20971528";

    public readonly static OrderBookRepository orderBookRepository = new();
    public readonly static TradeRepository tradeRepository = new();
    private static readonly LinearParser parser = new();
    static void Main()
    {   
        AllocConsole();

        WebSocketClient client = new WebSocketClient();
        client.ConnectToWebSocketAsync().Wait();
        _ = client.Subscribe(HandleMessages);

        Console.WriteLine("Input an Instrument Reference or write 'exit' to exit.");

        string? command;
        while (true)
        {
            Console.Write("> ");
            command = Console.ReadLine();

            if (command == null)
                continue;

            if (int.TryParse(command, out int insRef))
            {
                // Open a new window on a separate thread
                Thread thread = new Thread(() =>
                {
                    Application.Run(new MyForm(insRef));
                });
                thread.SetApartmentState(ApartmentState.STA); // UI thread must be STA
                thread.Start();
            }
            else if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                client.Close().Wait();
                break;
            }
            else
            {
                Console.WriteLine("Unknown command. Input an Instrument Reference or write 'exit'.");
            }
        }
    }

    private static void HandleMessages(string msg)
    {
        parser.Load(msg);
        while(parser.HasNextMessage())
        {
            string? message = parser.GetNextMessage().Replace(',', ' ');
            string[]? lines = message?.Split('\n');
            string? mclass = lines?[0].Split(':')[1].Trim();
            string? insref = lines?[1].Split(":")[1].Trim();
            switch (mclass)
            {
                case MC_ORDER:
                case MC_ORDER_BOOK_FLUSH:
                    HandleOrderMessage(message); break;
                case MC_TRADE:
                    HandleTradeMessage(message); break;
                default: break;
            }
        }
    }

    private static void HandleOrderMessage(string? message)
    {
        OrderEvent orderEvent = OrderEventMapper.MapToOrderEvent(message);
        orderBookRepository.HandleEvent(orderEvent);
    }

    private static void HandleTradeMessage(string? message)
    {
        Trade trade = TradeMapper.MapToTrade(message);
        tradeRepository.Add(trade);
    }
}