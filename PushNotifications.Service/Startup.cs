using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PushNotifications.Service.Hubs;

namespace PushNotifications.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<Worker>();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => 
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:3000")
                .AllowCredentials();
            }));

            services.AddSignalR();

            services.AddMediatR(typeof(Program));

            services.AddEventService();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Push Notification Service Online!");
                });

                endpoints.MapHub<PushNotificationHub>("/notificationHub");
            });
        }
    }
}
