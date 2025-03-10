using SendMessage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseWebSockets();

//await Generator.GenerateTestDataAndGetUpdatesAsync(100);

app.Map("/frontEnd-webSocket", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        DateTime startTime = DateTime.Now;

        await context.GenerateTestDataAndGetUpdatesAsync(5000);

        DateTime endTime = DateTime.Now;

        TimeSpan difference = endTime - startTime;

        Console.WriteLine($"Time taken {(int)difference.TotalMinutes} minute {(int)difference.TotalSeconds} seconds ");
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();







