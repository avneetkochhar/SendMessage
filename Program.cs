using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");


//app.UseRouting();
//app.UseAuthorization();
//app.MapControllers();
app.UseWebSockets();




app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await SendUpdates(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

async Task SendUpdates(WebSocket webSocket)
{
    while (webSocket.State == WebSocketState.Open)
    {
        var status = new
        {
            name = "My .NET Service",
            online = DateTime.UtcNow.Second % 2 == 0, // Simulated random status
            timestamp = DateTime.UtcNow
        };
        string jsonMessage = System.Text.Json.JsonSerializer.Serialize(status);
        var buffer = Encoding.UTF8.GetBytes(jsonMessage);
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);      
        await Task.Delay(1000); // Send updates every 1 seconds
    }
}

app.Run();
