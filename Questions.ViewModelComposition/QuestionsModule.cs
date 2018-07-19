using ITOps.Json;
using ITOps.ViewModelComposition.Modules;
using Questions.ViewModelComposition.Events;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;

namespace Questions.ViewModelComposition
{
    public class QuestionsModule : RequestsModule
    {
        public QuestionsModule()
        {
            Get("/questions", async (requestId, vm) =>
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
            });

            Get("/questions/{id}", async (requestId, vm) =>
            {
                var id = RouteData.Values["id"].ToString();
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
            });
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
