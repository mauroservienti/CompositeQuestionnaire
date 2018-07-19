//using ITOps.Json;
//using ITOps.ViewModelComposition;
//using Questions.ViewModelComposition.Events;
//using System.Net.Http;

//namespace Rules.ViewModelComposition
//{
//    public class QuestionsLoadedSubscriber : EventSubscriber<QuestionsLoaded>
//    {
//        public QuestionsLoadedSubscriber()
//        {
//            Get("/questions", async (requestId, vm, @event) =>
//            {
//                var questionIds = string.Join(",", @event.QuestionsViewModel.Keys);
//                var url = $"http://localhost:54219/api/rules?questionIds={questionIds}";
//                var client = new HttpClient();
//                var response = await client.GetAsync(url);
//                dynamic[] rules = await response.Content.AsExpandoArray();

//                foreach (dynamic rule in rules)
//                {
//                    dynamic question = @event.QuestionsViewModel[rule.QuestionId];
//                    question.CorrectAnswerId = rule.CorrectAnswerId;
//                }
//            });
//        }
//    }
//}
