using Microsoft.Extensions.DependencyInjection;
using System;

namespace NServiceBus.Extensions
{
    public static class ServiceBus
    {
        public static void AddNServiceBus(this IServiceCollection services, string endpointName, Action<EndpointConfiguration> customize = null)
        {
            var config = new EndpointConfiguration(endpointName);
            config.UseSerialization<NewtonsoftSerializer>();
            config.UseTransport<LearningTransport>();
            config.SendFailedMessagesTo("error");

            var messageConventions = config.Conventions();
            messageConventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Messages.Events"));
            messageConventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Messages.Commands"));
            messageConventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith(".Messages"));

            customize?.Invoke(config);

            var instance = Endpoint.Start(config).GetAwaiter().GetResult();
            services.AddSingleton<IMessageSession>(instance);
        }
    }
}