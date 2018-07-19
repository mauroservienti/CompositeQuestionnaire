//using ITOps.Json;
//using ITOps.ViewModelComposition;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Routing;
//using System.Collections.Generic;
//using System.Dynamic;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Answers.ViewModelComposition
//{
//    public class QuestionAnswersGetHandler : IHandleRequests
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
//            var url = $"http://localhost:54218/api/answers/?questionIds={id}";
//            var client = new HttpClient();
//            var response = await client.GetAsync(url);

//            dynamic[] answers = await response.Content.AsExpandoArray();

//            vm.Answers = new List<dynamic>();
//            foreach (dynamic answer in answers)
//            {
//                dynamic answerViewModel = new ExpandoObject();
//                answerViewModel.AnswerId = answer.AnswerId;
//                answerViewModel.AnswerText = answer.AnswerText;
//                answerViewModel.QuestionId = answer.QuestionId;
//                vm.Answers.Add(answerViewModel);
//            }
//        }
//    }
//}
