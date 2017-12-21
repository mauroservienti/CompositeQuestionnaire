using ITOps.Json;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.Http;
using System.Threading.Tasks;

namespace Questions.ViewModelComposition
{
    public class QuestionGetHandler : IHandleRequests
    {
        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
        {
            var controller = (string)routeData.Values["controller"];
            var action = (string)routeData.Values["action"];

            return HttpMethods.IsGet(httpVerb)
                   && controller.ToLowerInvariant() == "questions"
                   && string.IsNullOrWhiteSpace(action)
                   && routeData.Values.ContainsKey("id");
        }

        public async Task Handle(string requestId, dynamic vm, RouteData routeData, HttpRequest request)
        {
            var id = routeData.Values["id"].ToString();
            var url = $"http://localhost:54217/api/questions/{id}";
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            dynamic question = await response.Content.AsExpando();

            if (question == null)
            {
                //no way, yet, to set a response statuis code as 404
                return;
            }

            vm.QuestionId = question.QuestionId;
            vm.QuestionText = question.QuestionText;
        }
    }
}
