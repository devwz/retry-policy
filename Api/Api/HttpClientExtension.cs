using Polly;
using Polly.Extensions.Http;
using Refit;

namespace Api
{
    public static class HttpClientExtension
    {
        public static void RegisterHttpClient(this IServiceCollection services)
        {
            services.RegisterClient<IApi>("http://127.0.0.1:3000");
        }

        readonly static RefitSettings refitSettings = new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new System.Text.Json.JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
                })
        };

        static IHttpClientBuilder RegisterClient<T>(
            this IServiceCollection services, string url)
            where T : class
        {
            return services.Register<T>(url);
        }

        static IHttpClientBuilder Register<T>(
            this IServiceCollection services, string url)
            where T : class
        {
            try
            {
                return services
                    .AddRefitClient<T>(refitSettings)
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri(url);
                    })
                    .AddPolicyHandler(AddPolicies());
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        private static IAsyncPolicy<HttpResponseMessage> AddRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    2,
                    retryAttempt => TimeSpan.FromSeconds(2)
                );
        }

        private static IAsyncPolicy<HttpResponseMessage> AddCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    6,
                    TimeSpan.FromSeconds(10)
                );

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(6, TimeSpan.FromSeconds(10),
                    onBreak: (result, time) =>
                    {

                    },
                    onReset: () =>
                    {

                    },
                    onHalfOpen: () =>
                    {
                        
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> AddTimeoutPolicy()
        {
            return Policy
                .TimeoutAsync<HttpResponseMessage>(
                    TimeSpan.FromSeconds(20)
                );
        }

        private static IAsyncPolicy<HttpResponseMessage> AddPolicies()
        {
            var policies = new List<IAsyncPolicy<HttpResponseMessage>>
            {
                AddRetryPolicy(),
                AddCircuitBreakerPolicy(),
                AddTimeoutPolicy()
            };

            policies.RemoveAll(p => p == null);
            return Policy.WrapAsync(policies.ToArray());
        }
    }
}
