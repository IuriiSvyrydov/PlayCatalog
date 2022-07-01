
using MassTransit;
using MassTransit.MultiBus;
using PlayCatalog.API.Settings;
using PlayCatalog.Application.Extensions;
using PlayCatalog.Model;
using ServiceSettings = PlayCatalog.Application.Settings.ServiceSettings;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var _settings =builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        config.Host(rabbitMqSettings.Host);
        config.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(_settings.ServiceName, false));
    });
});
builder.Services.AddSingleton<IHostedService, MassTransitHostedService>();
builder.Services.AddControllers(opt =>
{
    opt.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigurationMongoSettigs(builder.Configuration).AddMongoRepository<Item>("items");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();