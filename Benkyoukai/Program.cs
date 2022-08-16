using Benkyoukai.Data;
using Benkyoukai.Services.Sessions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(
    builder.Configuration.GetValue<string>("Database:ConnectionString")))
    .AddSingleton<DatabaseInitializer>()
    .AddSingleton<ISessionService, SessionService>();

var app = builder.Build();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
