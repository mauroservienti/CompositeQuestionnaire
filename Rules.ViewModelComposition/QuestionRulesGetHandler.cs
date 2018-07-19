//using ITOps.Json;
//using ITOps.ViewModelComposition;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Routing;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Rules.ViewModelComposition
//{
//    public class QuestionRulesGetHandler : IHandleRequests
//    {
//        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
//        {
//            var controller = (string)routeData.Values["controller"];
//            var action = (string)routeData.Values["action"];

//            return HttpMethods.IsGet(httpVerb)
//                   && controller.ToLowerInvariant() == "questions"
//                   && string.IsNullOrWhiteSpace(action)
//                   && routeData.Values.ContainsKey("id");
//        }

//        public async Task Handle(string requestId, dynamic vm, RouteData routeData, HttpRequest request)
//        {
//            var id = routeData.Values["id"].ToString();
//            var url = $"http://localhost:54219/api/rules/?questionIds={id}";
//            var client = new HttpClient();
//            var response = await client.GetAsync(url);

//            //we know there is only one per question
//            dynamic rule = (await response.Content.AsExpandoArray()).Single();
//            vm.Rule = rule;
//        }
//    }
//}
