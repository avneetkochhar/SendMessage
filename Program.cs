using SendMessage;
using SendMessage.Models;
using SendMessage.Services;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

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
        await GenerateTestDataAndGetUpdatesAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

 static async Task GenerateTestDataAndGetUpdatesAsync(WebSocket webSocket)
{
    const int numberOfMessages = 100;
    Account[] accountArray = numberOfMessages.GenerateData();
   
    foreach (Account account in accountArray)
    {

        foreach (HttpBody httpBody in account.httpMessages)
        {
            account.accountId.sendMessageWithValidLimit(httpBody);
        }

        Object status = account.accountId.GetDetails();

        string jsonMessage =JsonSerializer.Serialize(status);

        var buffer = Encoding.UTF8.GetBytes(jsonMessage);

        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

        Console.WriteLine(status);
    }      
}



app.Run();




