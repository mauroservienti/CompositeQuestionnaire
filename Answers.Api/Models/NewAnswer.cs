using System;

namespace Answers.Api.Models
{
    public class NewAnswer
    {
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
    }
}
