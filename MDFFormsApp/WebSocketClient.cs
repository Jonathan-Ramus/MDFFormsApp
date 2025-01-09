namespace MDFFormsApp;

using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class WebSocketClient
{
    private const int BUF_SIZE = 8192;
    private readonly Uri serverUri = new Uri(""); //Scrubbed for security
    private readonly ClientWebSocket ws;
    private Task? stream;
    public WebSocketClient()
    {
        ws = new ClientWebSocket();
    }
    public async Task ConnectToWebSocketAsync()
    {
        try
        {
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Console.WriteLine("Connected to WebSocket server!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task Subscribe(Action<string> callback)
    {
        try
        {
            string messageToSend = "subscribe";
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageToSend);
            await ws.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            stream = Task.Run(() => StreamData(ws, callback));
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task Close()
    {   
        stream?.Dispose();
        
        string messageToSend = "unsubscribe";
        byte[] messageBytes = Encoding.UTF8.GetBytes(messageToSend);
        await ws.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        Console.WriteLine("WebSocket connection closed.");
    }

    private static async void StreamData(ClientWebSocket ws, Action<string> callback)
    {
        byte[] buffer = new byte[BUF_SIZE];
        StringBuilder messageBuilder = new StringBuilder();
        WebSocketReceiveResult result;

        for (; ; )
        {
            messageBuilder.Clear();
            do
            {
                result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
            }
            while (result.MessageType != WebSocketMessageType.Close && !result.EndOfMessage);

            callback(messageBuilder.ToString());
        }
    }
}