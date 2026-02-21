using CloudNative.ConfigLibrary;
using CloudNative.ConfigLibrary.KafkaServices;
using CloudNative.ConfigLibrary.Settings;
using CloudNative.Customer.Api.Middleware;
using CloudNative.Customer.Application;
using CloudNative.Customer.Application.Features.Customer.Consumers;
using CloudNative.Customer.Core.Constants;
using CloudNative.Customer.Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

string CorsAll = "AllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsAll, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddConfigLibaray()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService(sp =>
{
    var kafkaSettings = sp
        .GetRequiredService<IOptions<KafkaSettings>>()
        .Value;

    var consumerInfo = KafkaRegistry.Consumers
      .First(x => x.Topic == "customer-topic");

    return new KafkaConsumerService(
        bootstrapServers: kafkaSettings.BootstrapServers,
        topic: consumerInfo.Topic,
        groupId: consumerInfo.Group,
        processor: new CustomerListProcessor());
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(CorsAll);
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var topicManager =
    scope.ServiceProvider.GetRequiredService<KafkaTopicManager>();
    foreach (var consumer in KafkaRegistry.Consumers)
    {
        await topicManager.CreateTopicIfNotExistsAsync(consumer.Topic);
        await topicManager.CreateTopicIfNotExistsAsync(consumer.Topic + "-dlt");
    }
}

app.Run();
