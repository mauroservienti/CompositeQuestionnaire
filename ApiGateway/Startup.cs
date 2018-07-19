using ITOps.ViewModelComposition;
using ITOps.ViewModelComposition.Gateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus.Extensions;

namespace ApiGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddViewModelComposition();
            services.AddCors();
            services.AddNServiceBus(endpointName: "Api.Gateway");
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.Use((context, next) =>
            {
                context.Request.EnableRewind();
                return next();
            });

            app.UseCors(policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowAnyHeader();
            });

            app.RunCompositionGateway(routes =>
            {
                routes.MapComposableRoute(
                    template: "{*CatchAllToAllowNancyStyleHandlers}",
                    constraints: new RouteValueDictionary(new
                    {
                        httpMethod = new HttpMethodRouteConstraint(
                            HttpMethods.Get,
                            HttpMethods.Post,
                            HttpMethods.Put,
                            HttpMethods.Delete)
                    }));
            });
        }
    }
}