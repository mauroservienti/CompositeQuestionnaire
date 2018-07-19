//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Routing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ITOps.ViewModelComposition.Modules
//{
//    public abstract class EventSubscriber<TEvent> : ISubscribeToCompositionEvents, IHandleRequestsErrors
//    {
//        class Handler
//        {
//            internal Func<string, Exception, dynamic, Task> onError;
//            internal Func<string, dynamic, TEvent, Task> onHandle;
//        }

//        RouteMatcher routeMatcher = new RouteMatcher();
//        Dictionary<string, List<Handler>> gets = new Dictionary<string, List<Handler>>();
//        Dictionary<string, Handler[]> getsCache = new Dictionary<string, Handler[]>();

//        protected RouteData RouteData { get; private set; }
//        protected HttpRequest HttpRequest { get; private set; }
//        protected IQueryCollection Query { get; private set; }

//        protected void Get(string template, Func<string, dynamic, TEvent, Task> handler, Func<string, Exception, dynamic, Task> onError = null)
//        {
//            if (!gets.TryGetValue(template, out List<Handler> handlers))
//            {
//                handlers = new List<Handler>();
//                gets.Add(template, handlers);
//            }

//            handlers.Add(new Handler() { onHandle = handler, onError = onError });
//        }

//        bool IInterceptRoutes.Matches(RouteData routeData, string httpVerb, HttpRequest request)
//        {
//            RouteData = routeData;
//            HttpRequest = request;

//            switch (httpVerb.ToLowerInvariant())
//            {
//                case "get":
//                    {
//                        if (!getsCache.TryGetValue(request.Path, out Handler[] handlers))
//                        {
//                            handlers = gets.Where(kvp => routeMatcher.Match(kvp.Key, request.Path, routeData.DataTokens))
//                                .SelectMany(kvp => kvp.Value)
//                                .ToArray();
//                            getsCache.Add(request.Path, handlers);
//                        }

//                        return handlers.Any();
//                    }

//                default:
//                    return false;
//            }
//        }

//        void ISubscribeToCompositionEvents.Subscribe(IPublishCompositionEvents publisher)
//        {
//            publisher.Subscribe<TEvent>(async (requestId, vm, @event, routeData, query) =>
//            {
//                Query = query;
//                switch (HttpRequest.Method.ToLowerInvariant())
//                {
//                    case "get":
//                        {
//                            var handlers = getsCache[HttpRequest.Path];
//                            var pendingTasks = new List<Task>();
//                            foreach (var item in handlers)
//                            {
//                                pendingTasks.Add(item.onHandle(requestId, vm, @event));
//                            }

//                            await Task.WhenAll(pendingTasks);
//                        }
//                        break;
//                }
//            });
//        }

//        Task IHandleRequestsErrors.OnRequestError(string requestId, Exception ex, dynamic vm, RouteData routeData, HttpRequest request)
//        {
//            RouteData = routeData;
//            HttpRequest = request;

//            switch (request.Method.ToLowerInvariant())
//            {
//                case "get":
//                    {
//                        var handlers = getsCache[request.Path];
//                        var pendingTasks = new List<Task>();
//                        foreach (var item in handlers)
//                        {
//                            pendingTasks.Add(item.onError(requestId, ex, vm));
//                        }

//                        return Task.WhenAll(pendingTasks);
//                    }
//                default:
//                    throw new NotSupportedException();
//            }
//        }
//    }
//}
