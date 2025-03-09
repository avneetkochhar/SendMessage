using SendMessage;
using SendMessage.Models;
using SendMessage.Services;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<MessageController>(); // Register controller
var app = builder.Build();
var scope = app.Services.CreateScope();
var status =new HttpBody();

app.UseRouting();
app.MapControllers();
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
        var controller = scope.ServiceProvider.GetRequiredService<MessageController>(); 

        string jsonMessage = System.Text.Json.JsonSerializer.Serialize(controller.GetDataForWebSocket());
        var buffer = Encoding.UTF8.GetBytes(jsonMessage);
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);      
        await Task.Delay(1000); // Send updates every 1 seconds
    }
}

//app.Run();





const int numberOfMessages = 100;

foreach (Account account in numberOfMessages.generateData()) {

    foreach (HttpBody httpBody in account.httpMessages)
    {
        account.accountId.sendMessageWithValidLimit(httpBody);
    }
}





   

