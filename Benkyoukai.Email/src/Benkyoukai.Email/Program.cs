using Benkyoukai.Email.Data;
using Benkyoukai.Email.Options;
using Benkyoukai.Email.Services.Common;
using Benkyoukai.Email.Services.Email;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IMQConnectionFactory>(new RMQConnectionFactory(
        hostName: builder.Configuration.GetValue<string>("RabbitMQ:Hostname")!,
        userName: builder.Configuration.GetValue<string>("RabbitMQ:Username")!,
        password: builder.Configuration.GetValue<string>("RabbitMQ:Password")!));

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));
    builder.Services.AddSingleton<IEmailService, EmailService>()
        .AddSingleton<IMessageConsumer, MessageConsumer>()
        .AddSingleton<DeadEmailService>();
    builder.Services.AddHealthChecks();
}

var app = builder.Build();
{
    app.MapHealthChecks("/health");
}

var emailService = app.Services.GetRequiredService<IEmailService>();
var deadEmailService = app.Services.GetRequiredService<DeadEmailService>();
var messageConsumer = app.Services.GetRequiredService<IMessageConsumer>();

var emailRegistrationChannel = messageConsumer.OpenChannel(
    queue: "email",
    deadLetterRoutingKey: "dlx.email");
emailRegistrationChannel.Received += emailService.ProcessEmail(emailRegistrationChannel.Model);

var deadLetterChannel = messageConsumer.OpenChannel(
    queue: "dlx.email");
deadLetterChannel.Received += deadEmailService.ProcessDeadEmail(deadLetterChannel.Model);

app.Run();
