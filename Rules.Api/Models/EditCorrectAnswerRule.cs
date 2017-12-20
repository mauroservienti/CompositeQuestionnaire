using System;

namespace Rules.Api.Models
{
    public class EditCorrectAnswerRule
    {
        public Guid RuleId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid CorrectAnswerId { get; set; }
    }
}
