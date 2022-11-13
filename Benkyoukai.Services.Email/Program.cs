using System.Text;
using System.Text.Json;
using Benkyoukai.Services.Contracts.Email;
using Benkyoukai.Services.Email.Data;
using Benkyoukai.Services.Email.Options;
using Benkyoukai.Services.Email.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IMQConnectionFactory>(new RMQConnectionFactory(
        hostName: builder.Configuration.GetValue<string>("RabbitMQ:Hostname")!,
        userName: builder.Configuration.GetValue<string>("RabbitMQ:Username")!,
        password: builder.Configuration.GetValue<string>("RabbitMQ:Password")!));

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));
    builder.Services.AddSingleton<IEmailService, EmailService>();
    builder.Services.AddHealthChecks();
}

var app = builder.Build();
{
    app.MapHealthChecks("/health");
}


var connection = app.Services.GetRequiredService<IMQConnectionFactory>()
    .CreateConnection();

var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "email",
    durable: true,
    exclusive: false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(
    queue: "email",
    autoAck: false,
    consumer: consumer);

var emailService = app.Services.GetRequiredService<IEmailService>();

consumer.Received += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var email = JsonSerializer.Deserialize<EmailRegisterMessageDto>(message)!;

    var sent = await emailService.SendEmailAsync(email);
    if (!sent)
    {
        channel.BasicNack(eventArgs.DeliveryTag, false, true);
        return;
    }

    Console.WriteLine($"Received {message}");
    channel.BasicAck(eventArgs.DeliveryTag, false);
};

app.Run();
