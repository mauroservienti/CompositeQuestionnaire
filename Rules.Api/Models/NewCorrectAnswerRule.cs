using System;

namespace Rules.Api.Models
{
    public class NewCorrectAnswerRule
    {
        public Guid QuestionId { get; set; }
        public Guid CorrectAnswerId { get; set; }
    }
}
