using SendMessage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseWebSockets();

await Generator.GenerateTestDataAndSendSMS(25000);// pass a number to generate test messages in each account

app.Map("/frontEnd-webSocket", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {  
        await context.GenerateTestDataToSendSMSAndGetUpdatesWithWebSocket(25000);// pass a number to generate test messages in each account
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();







