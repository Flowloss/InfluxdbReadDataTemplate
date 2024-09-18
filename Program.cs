using Fleck;
using InfluxDBTest.Services;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<InfluxDBService>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        var server = new WebSocketServer("ws://0.0.0.0:8181");

        var wsConnections = new List<IWebSocketConnection>();

        server.Start(ws =>
        {
            ws.OnOpen = () =>
            { 
                wsConnections.Add(ws);
            };

            ws.OnMessage = message =>
            {
                foreach (var webSocketConnection in wsConnections)
                {
                    webSocketConnection.Send(message);
                }
            };
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}