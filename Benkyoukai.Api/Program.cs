using Benkyoukai.Api.Data;
using Benkyoukai.Api.Middlewares;
using Benkyoukai.Api.Services.Sessions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
            ?? builder.Configuration.GetValue<string>("ConnectionStrings:Db")));

    builder.Services.AddControllers();
    builder.Services.AddSingleton<DbInitializer>()
        .AddSingleton<ISessionService, SessionService>();

    builder.Services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

    builder.Services.AddHealthChecks();
}

var app = builder.Build();
{
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    app.MapControllers();
    app.MapHealthChecks("/health");
    app.MapGet("/", () => "Hello World!");
    
    app.UseSwagger();
    app.UseSwaggerUI();

    // var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
    // await dbInitializer.InitializeAsync();

    app.Run();
}

