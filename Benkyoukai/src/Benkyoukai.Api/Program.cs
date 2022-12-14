using System.Text;
using Benkyoukai.Api.Data;
using Benkyoukai.Api.Middlewares;
using Benkyoukai.Api.Repositories;
using Benkyoukai.Api.Services.Authentication;
using Benkyoukai.Api.Services.Common;
using Benkyoukai.Api.Services.Email;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                builder.Configuration.GetValue<string>("Token:Secret")!
            )),
            ValidIssuer = builder.Configuration.GetValue<string>("Token:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Token:Audience"),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization(b =>
    {
        b.AddPolicy("User", p =>
        {
            p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser();
        });
    });

    builder.Services.AddCors(opts =>
    {
        opts.AddDefaultPolicy(b =>
        {
            b.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:5173");
        });
    });

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    builder.Services.AddSingleton<IUserRepository, UserRepository>()
        .AddSingleton<ISessionRepository, SessionRepository>();

    builder.Services.AddScoped<IAuthService, AuthService>()
        .AddScoped<ITokenService, TokenService>()
        .AddScoped<IEmailService, EmailService>()
        .AddScoped<IMessageProducer, MessageProducer>();

    builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? builder.Configuration.GetValue<string>("Database:ConnectionString")!));
    builder.Services.AddSingleton<DbInitializer>();

    builder.Services.AddSingleton<IMQConnectionFactory>(_ => new RMQConnectionFactory(
        hostName: builder.Configuration.GetValue<string>("RabbitMQ:Hostname")!,
        userName: builder.Configuration.GetValue<string>("RabbitMQ:Username")!,
        password: builder.Configuration.GetValue<string>("RabbitMQ:Password")!
    ));

    builder.Services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddHealthChecks();
}

var app = builder.Build();
{
    app.UseCors();
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health").AllowAnonymous();
    app.MapGet("/", () => "Hello World!");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
    await dbInitializer.InitializeAsync();

    app.Run();
}
