using Benkyoukai.Api.Data;
using Benkyoukai.Api.Services.Sessions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(
    builder.Configuration.GetValue<string>("ConnectionStrings:Sqlite")));

builder.Services.AddControllers();
builder.Services.AddSingleton<IDbInitializer, SqliteDbInitilizer>()
    .AddSingleton<ISessionService, SessionService>();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();


var app = builder.Build();

app.UseExceptionHandler("/error");
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

// var dbInitializer = app.Services.GetRequiredService<IDbInitializer>();
// await dbInitializer.InitializeAsync();

app.Run();