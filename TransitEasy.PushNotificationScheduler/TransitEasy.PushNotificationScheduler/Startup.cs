using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Http.Headers;
using TransitEasy.NotificationScheduler.Core.Clients;
using TransitEasy.NotificationScheduler.Core.Handlers;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.NotificationScheduler.Core.Models.Result;
using TransitEasy.NotificationScheduler.Core.Options;

namespace TransitEasy.PushNotificationScheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransitEasy Push Notification API", Version = "v1" }));

            //logging 
            services.AddLogging();

            //service options
            services.Configure<ApplicationOptions>(Configuration);

            //api clients
            services.AddTransient<IGoogleCloudTaskApiClient, GoogleCloudTaskApiClient>();
            services.AddTransient<ITransitEasyApiClient, TransitEasyApiClient>();
            services.AddTransient<IFirebaseApiClient, FirebaseApiClient>(); 

            //request handlers
            services.AddTransient<IRequestHandler<CreateNotificationRequest, CreateNotificationResponse>, SchedulerRequestHandler>();
            services.AddTransient<IRequestHandler<SendNotificationRequest, SendNotificationResponse>, SendNotificationRequestHandler>();

            //http clients 
            services.AddHttpClient("TransitEasyApiClient", (svp, client) => {
                var options = svp.GetRequiredService<IOptionsMonitor<ApplicationOptions>>();
                client.BaseAddress = new Uri(options.CurrentValue.TransitEasyApiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.CurrentValue.TransitEasyApiTimeoutInSec);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            //health check
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            app.UseSwagger()
                .UseSwaggerUI((c => c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName)))
                .UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName);
                });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health");
            });

        }
    }
}
