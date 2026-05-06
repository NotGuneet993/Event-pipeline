using Elastic.Clients.Elasticsearch;
using Consumer.Services;
using EtwDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(3);
});

// Elasticsearch
builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var settings = new ElasticsearchClientSettings(new Uri(config["Elasticsearch:Url"] ?? ""))
        .DefaultIndex(config["Elasticsearch:DefaultIndex"] ?? "");
    return new ElasticsearchClient(settings);
});

// Add services for DI
builder.Services.AddSingleton<ElasticsearchService>();
builder.Services.AddSingleton<WebSocketService>();

// 5 kafka consumers
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddHostedService<KafkaConsumerService>();

var app = builder.Build();

app.UseWebSockets();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();  
