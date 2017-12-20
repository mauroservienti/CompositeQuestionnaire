using System;

namespace Answers.Api.Models
{
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
    }
}
