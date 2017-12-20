using ITOps.Json;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Questions.ViewModelComposition.Events;
using System.Net.Http;

namespace Rules.ViewModelComposition
{
    public class QuestionsLoadedSubscriber : ISubscribeToCompositionEvents
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

        public void Subscribe(IPublishCompositionEvents publisher)
        {
            publisher.Subscribe<QuestionsLoaded>(async (vm, @event, routeData, query) =>
            {
                var questionIds = string.Join(",", @event.QuestionsViewModel.Keys);
                var url = $"http://localhost:54219/api/rules?questionIds={questionIds}";
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                dynamic[] rules = await response.Content.AsExpandoArray();

                foreach (dynamic rule in rules)
                {
                    dynamic question = @event.QuestionsViewModel[rule.QuestionId];
                    question.CorrectAnswerId = rule.CorrectAnswerId;
                }
            });
        }
    }
}
