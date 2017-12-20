using System.Collections.Generic;

namespace Questions.ViewModelComposition.Events
{
    public class QuestionsLoaded
    {
        public IDictionary<dynamic, dynamic> QuestionsViewModel { get; set; }
    }
}
