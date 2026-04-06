using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PartnerTransactionHub.Api.Configuration;
using PartnerTransactionHub.Api.Middlewares;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TTSCo.Services;
using TTSCo.Validators;
using TTSCom.Configuration;
using TTSCom.Messaging;

namespace PartnerTransactionHub.Api
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
            services.Configure<PartnerApiSettings>(
                    Configuration.GetSection("PartnerApi"));

            services.AddHttpClient<IPartnerVerificationService, PartnerVerificationService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<PartnerApiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

            services.Configure<RabbitMqSettings>(
                    Configuration.GetSection("RabbitMq"));

            services.Configure<ApiKeySettings>(
                    Configuration.GetSection("Security"));

            services.AddControllers()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TransactionRequestValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (result, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount}");
                    });
        }
    }
}
