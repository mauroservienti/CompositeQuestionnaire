using ITOps.Json;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Answers.ViewModelComposition
{
    public class QuestionAnswersPutHandler : IHandleRequests, IHandleRequestsErrors
    {
        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
        {
            var controller = (string)routeData.Values["controller"];
            var action = (string)routeData.Values["action"];

            return HttpMethods.IsPut(httpVerb)
                   && controller.ToLowerInvariant() == "questions"
                   && string.IsNullOrWhiteSpace(action)
                   && !routeData.Values.ContainsKey("id");
        }

        public async Task Handle(string requestId, dynamic vm, RouteData routeData, HttpRequest request)
        {
            dynamic putData = request.Body.AsExpando();
            var answers = new List<dynamic>();
            foreach (dynamic answer in putData.Answers)
            {
                answers.Add(new
                {
                    answer.QuestionId,
                    answer.AnswerId,
                    answer.AnswerText
                });
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:54218/api/answers")
            {
                Content = new StringContent(JsonConvert.SerializeObject(answers), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("composed-request-id", requestId);

            var response = await new HttpClient()
                .SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(response.ReasonPhrase);
            }
        }

        public Task OnRequestError(string requestId, Exception ex, dynamic vm, RouteData routeData, HttpRequest request)
        {
            return Task.CompletedTask;
        }
    }
}
