using Benkyoukai.Services.Email.Data;
using Benkyoukai.Services.Email.Options;
using Benkyoukai.Services.Email.Services.Common;
using Benkyoukai.Services.Email.Services.Email;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IMQConnectionFactory>(new RMQConnectionFactory(
        hostName: builder.Configuration.GetValue<string>("RabbitMQ:Hostname")!,
        userName: builder.Configuration.GetValue<string>("RabbitMQ:Username")!,
        password: builder.Configuration.GetValue<string>("RabbitMQ:Password")!));

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));
    builder.Services.AddSingleton<IEmailService, EmailService>()
        .AddSingleton<IMessageConsumer, MessageConsumer>();
    builder.Services.AddHealthChecks();
}

var app = builder.Build();
{
    app.MapHealthChecks("/health");
}

var emailService = app.Services.GetRequiredService<IEmailService>();
var messageConsumer = app.Services.GetRequiredService<IMessageConsumer>();

var consumer = messageConsumer.CreateEmailRegistrationChannel();
consumer.Received += emailService.ProcessMessage(consumer.Model);

app.Run();
