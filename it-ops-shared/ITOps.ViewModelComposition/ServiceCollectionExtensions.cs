using ITOps.ViewModelComposition.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ITOps.ViewModelComposition
{
    public static class ServiceCollectionExtensions
    {
        public static void AddViewModelComposition(this IServiceCollection services, string assemblySearchPattern = "*ViewModelComposition*.dll")
        {
            var fileNames = Directory.GetFiles(AppContext.BaseDirectory, assemblySearchPattern);

            var routeInterceptors = new List<Type>();
            var modules = new List<Type>();

            foreach (var fileName in fileNames)
            {
                var types = AssemblyLoader.Load(fileName).GetTypes();
                routeInterceptors.AddRange(types.Where(t =>
                {
                    var typeInfo = t.GetTypeInfo();
                    return !typeInfo.IsInterface
                        && !typeInfo.IsAbstract
                        && typeof(IInterceptRoutes).IsAssignableFrom(t);
                }));

                modules.AddRange(types.Where(t =>
                {
                    var typeInfo = t.GetTypeInfo();
                    return !typeInfo.IsInterface
                        && !typeInfo.IsAbstract
                        && typeof(RequestsModule).IsAssignableFrom(t);
                }));
            }

            foreach (var type in routeInterceptors)
            {
                services.AddScoped(typeof(IInterceptRoutes), type);
            }

            foreach (var type in modules)
            {
                services.AddSingleton(typeof(RequestsModule), type);
            }
        }
    }
}
