using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPOI.SS.UserModel.MissingCellPolicy;

namespace VNPTNET.NOM.Lib
{
    public static class HttpClientRetryPolicy
    {
        public static IHttpClientBuilder UseImmediateHttpRetryPolicy(this IHttpClientBuilder builder,
            int retryCount = 3)
        {
            return builder.AddPolicyHandler(ConfigureImmediateHttpRetry(retryCount));
        }

        public static IHttpClientBuilder UseLinearHttpRetryPolicy(this IHttpClientBuilder builder,
            int retryCount = 3)
        {
            return builder.AddPolicyHandler(ConfigureLinearHttpRetry(retryCount));
        }

        public static IHttpClientBuilder UseExponentialHttpRetryPolicy(this IHttpClientBuilder builder,
            int retryCount = 5)
        {
            return builder.AddPolicyHandler(ConfigureExponentialHttpRetry(retryCount));
        }

        public static IHttpClientBuilder UseCircuitBreakerPolicy(this IHttpClientBuilder builder,
            int eventsAllowedBeforeBreaking = 3, int fromSeconds = 30)
        {
            return builder.AddPolicyHandler(ConfigureCircuitBreakerPolicy(eventsAllowedBeforeBreaking, fromSeconds));
        }

        public static IHttpClientBuilder ConfigureTimeoutPolicy(this IHttpClientBuilder builder, int seconds = 5)
        {
            return builder.AddPolicyHandler(Polly.Policy.TimeoutAsync<HttpResponseMessage>(seconds));
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureCircuitBreakerPolicy(int eventsAllowedBeforeBreaking, int fromSeconds)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: eventsAllowedBeforeBreaking,
                    durationOfBreak: TimeSpan.FromSeconds(fromSeconds)
                );
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureImmediateHttpRetry(int retryCount) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .RetryAsync(retryCount, (exception, retryCount, context) =>
                {
                    //Log.Error($"Retry {retryCount} of {context.PolicyKey} at " +
                    //          $"{context.OperationKey}, due to: {exception.Exception.Message}");
                });

        private static IAsyncPolicy<HttpResponseMessage> ConfigureLinearHttpRetry(int retryCount) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(3),
                    (exception, retryCount, context) =>
                    {
                        //Log.Error($"Retry {retryCount} of {context.PolicyKey} at " +
                        //      $"{context.OperationKey}, due to: {exception.Exception.Message}");
                    });


        private static IAsyncPolicy<HttpResponseMessage> ConfigureExponentialHttpRetry(int retryCount) =>
            // In this case will wait for
            //  2 ^ 1 = 2 seconds then
            //  2 ^ 2 = 4 seconds then
            //  2 ^ 3 = 8 seconds then
            //  2 ^ 4 = 16 seconds then
            //  2 ^ 5 = 32 seconds
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt
                        => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, retryCount, context) =>
                    {
                        //Log.Error($"Retry {retryCount} of {context.PolicyKey} at " +
                        //      $"{context.OperationKey}, due to: {exception.Exception.Message}");
                    });
    }
}
