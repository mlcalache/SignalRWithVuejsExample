using System;

namespace SignalRExample.API.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Body { get; set; }
    }
}