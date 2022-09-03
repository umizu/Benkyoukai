using Benkyoukai.Data;
using Benkyoukai.Services.Sessions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(
    builder.Configuration.GetValue<string>("Database:ConnectionString")));

builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseInitializer>()
    .AddSingleton<ISessionService, SessionService>();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();


var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
