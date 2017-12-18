﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace ITOps.ViewModelComposition
{
    public interface IHandleRequestsErrors : IInterceptRoutes
    {
        Task OnRequestError(string requestId, Exception ex, dynamic vm, RouteData routeData, HttpRequest request);
    }
}
