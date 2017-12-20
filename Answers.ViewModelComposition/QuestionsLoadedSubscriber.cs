using ITOps.Json;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Questions.ViewModelComposition.Events;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;

namespace Answers.ViewModelComposition
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
                var url = $"http://localhost:54218/api/answers?questionIds={questionIds}";
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                dynamic[] answers = await response.Content.AsExpandoArray();

                foreach (dynamic answer in answers)
                {
                    dynamic question = @event.QuestionsViewModel[answer.QuestionId];
                    if (!Dynamic.HasOwnProperty(question, "Answers"))
                    {
                        question.Answers = new List<dynamic>();
                    }

                    dynamic answerViewModel = new ExpandoObject();
                    answerViewModel.AnswerId = answer.AnswerId;
                    answerViewModel.AnswerText = answer.AnswerText;
                    question.Answers.Add(answerViewModel);
                }
            });
        }
    }
}
