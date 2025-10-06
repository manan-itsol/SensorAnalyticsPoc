using SensorAnalyticsPoc.Api.Data;
using SensorAnalyticsPoc.Api.HostedServices;
using SensorAnalyticsPoc.Api.Hubs;
using SensorAnalyticsPoc.Api.Models;
using StackExchange.Redis;
using System.Threading.Channels;

namespace SensorAnalyticsPoc.Api
{
    public class Program
    {
        const string AllowReactDevCorsPlicyName = "AllowReactDev";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSignalR();
            builder.Services.AddSingleton(Channel.CreateUnbounded<List<SensorReading>>());
            builder.Services.AddSingleton<ISensorDataStore, RedisSortedSetDataStore>();
            builder.Services.AddHostedService<SensorSimulatorService>();
            builder.Services.AddHostedService<RedisWriterHostedService>();
            builder.Services.AddHostedService<RedisPurgerHostedService>();
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowReactDevCorsPlicyName, policy =>
                {
                    policy.WithOrigins(builder.Configuration["FrontEndAppUrl"])
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(AllowReactDevCorsPlicyName);

            app.MapHub<SensorHub>("/hubs/sensor");

            app.Run();
        }
    }
}
