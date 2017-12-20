using ITOps.Json;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Questions.ViewModelComposition.Events;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Questions.ViewModelComposition
{
    public class QuestionsGetHandler : IHandleRequests
    {
        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
        {
            var controller = (string)routeData.Values["controller"];
            var action = (string)routeData.Values["action"];

            return HttpMethods.IsGet(httpVerb)
                   && controller.ToLowerInvariant() == "questions"
                   && string.IsNullOrWhiteSpace(action)
                   && !routeData.Values.ContainsKey("id");
        }

        public async Task Handle(string requestId, dynamic vm, RouteData routeData, HttpRequest request)
        {
            var url = $"http://localhost:54217/api/questions";
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            dynamic[] questions = await response.Content.AsExpandoArray();

            if (questions.Length == 0)
            {
                vm.Questions = questions;
                return;
            }

            IDictionary<dynamic, dynamic> questionsViewModel = MapToDictionary(questions);

            await vm.RaiseEvent(new QuestionsLoaded()
            {
                QuestionsViewModel = questionsViewModel
            });

            vm.Questions = questionsViewModel.Values.ToList();
        }

        IDictionary<dynamic, dynamic> MapToDictionary(IEnumerable<object> questions)
        {
            var questionsViewModel = new Dictionary<dynamic, dynamic>();

            foreach (dynamic item in questions)
            {
                dynamic vm = new ExpandoObject();
                vm.QuestionId = item.QuestionId;
                vm.QuestionText = item.QuestionText;

                questionsViewModel[item.QuestionId] = vm;
            }

            return questionsViewModel;
        }
    }
}
