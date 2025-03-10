using SendMessage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseWebSockets();

await Generator.GenerateTestDataAndSendSMS(250);

app.Map("/frontEnd-webSocket", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {        
        await context.GenerateTestDataToSendSMSAndGetUpdatesWithWebSocket(20000);// maximum limit a phone allow to send messages
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();







