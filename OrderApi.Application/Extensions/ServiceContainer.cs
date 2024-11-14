using Ecommerce.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interface;
using OrderApi.Application.Service;
using Polly;
using Polly.Retry;

namespace OrderApi.Application.Extensions
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // register httpclient service
            services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(2);
            });

            // create retry strategy
            var retry = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome{args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;

                }
            };
            // use retry strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retry);
            });

            return services;
        }
    }
}
