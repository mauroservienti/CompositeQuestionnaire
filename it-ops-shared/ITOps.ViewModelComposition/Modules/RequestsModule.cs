using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITOps.ViewModelComposition.Modules
{
    public abstract class RequestsModule //: IHandleRequests, IHandleRequestsErrors
    {
        class Handler
        {
            internal Func<string, dynamic, Task> onExecute;
            internal Func<string, Exception, dynamic, Task> onError;
        }

        RouteMatcher routeMatcher = new RouteMatcher();
        Dictionary<string, List<Handler>> gets = new Dictionary<string, List<Handler>>();
        Dictionary<string, Handler[]> getsCache = new Dictionary<string, Handler[]>();

        public void RegisterRoutes(RouteBuilder routeBuilder)
        {
            throw new NotImplementedException();
        }

        protected RouteData RouteData { get; private set; }
        protected HttpRequest HttpRequest { get; private set; }

        protected void Get(string template, Func<string, dynamic, Task> get, Func<string, Exception, dynamic, Task> onError = null)
        {
            if (!gets.TryGetValue(template, out List<Handler> handlers))
            {
                handlers = new List<Handler>();
                gets.Add(template, handlers);
            }

            handlers.Add(new Handler() { onExecute = get, onError = onError });
        }

        //bool IInterceptRoutes.Matches(RouteData routeData, string httpVerb, HttpRequest request)
        //{
        //    RouteData = routeData;
        //    HttpRequest = request;

        //    switch (httpVerb.ToLowerInvariant())
        //    {
        //        case "get":
        //            {
        //                if (!getsCache.TryGetValue(request.Path, out Handler[] handlers))
        //                {
        //                    handlers = gets.Where(kvp => routeMatcher.Match(kvp.Key, request.Path, routeData.DataTokens))
        //                        .SelectMany(kvp => kvp.Value)
        //                        .ToArray();
        //                    getsCache.Add(request.Path, handlers);
        //                }

        //                return handlers.Any();
        //            }

        //        default:
        //            return false;
        //    }
        //}

        //Task IHandleRequests.Handle(string requestId, dynamic vm, RouteData routeData, HttpRequest request)
        //{
        //    switch (request.Method.ToLowerInvariant())
        //    {
        //        case "get":
        //            {
        //                var handlers = getsCache[request.Path];
        //                var pendingTasks = new List<Task>();
        //                foreach (var item in handlers)
        //                {
        //                    pendingTasks.Add(item.onExecute(requestId, vm));
        //                }

        //                return Task.WhenAll(pendingTasks);
        //            }
        //        default:
        //            throw new NotSupportedException();
        //    }
        //}

        //Task IHandleRequestsErrors.OnRequestError(string requestId, Exception ex, dynamic vm, RouteData routeData, HttpRequest request)
        //{
        //    RouteData = routeData;
        //    HttpRequest = request;

        //    switch (request.Method.ToLowerInvariant())
        //    {
        //        case "get":
        //            {
        //                var handlers = getsCache[request.Path];
        //                var pendingTasks = new List<Task>();
        //                foreach (var item in handlers)
        //                {
        //                    pendingTasks.Add(item.onError(requestId, ex, vm));
        //                }

        //                return Task.WhenAll(pendingTasks);
        //            }
        //        default:
        //            throw new NotSupportedException();
        //    }
        //}
    }

    public class RouteMatcher
    {
        public bool Match(string routeTemplate, string requestPath, RouteValueDictionary routeValueDictionary)
        {
            var template = TemplateParser.Parse(routeTemplate);

            var matcher = new TemplateMatcher(template, GetDefaults(template));

            var match = matcher.TryMatch(requestPath, routeValueDictionary);

            return match;
        }

        // This method extracts the default argument values from the template.
        private RouteValueDictionary GetDefaults(RouteTemplate parsedTemplate)
        {
            var result = new RouteValueDictionary();

            foreach (var parameter in parsedTemplate.Parameters)
            {
                if (parameter.DefaultValue != null)
                {
                    result.Add(parameter.Name, parameter.DefaultValue);
                }
            }

            return result;
        }
    }
}
