using ITOps.ViewModelComposition;
using ITOps.ViewModelComposition.Gateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApiGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddViewModelComposition();
            services.AddCors();
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
                routes.MapComposableGet(template: "{controller}/{id:guid?}");
                routes.MapComposablePost(template: "{controller}/{id:guid}");
                routes.MapComposablePut(template: "{controller}");
                routes.MapComposableDelete(template: "{controller}/{id:guid}");
                routes.MapRoute("{*NotFound}", context =>
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return Task.CompletedTask;
                });
            });
        }
    }
}