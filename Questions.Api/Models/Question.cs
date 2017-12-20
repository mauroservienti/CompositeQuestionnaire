using System;

namespace Questions.Api.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}
