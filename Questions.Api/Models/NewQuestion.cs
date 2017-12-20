using System;

namespace Questions.Api.Models
{
    public class NewQuestion
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}
