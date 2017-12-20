using System;

namespace Questions.Api.Models
{
    public class EditQuestion
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}
