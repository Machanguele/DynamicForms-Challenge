using System;

namespace Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime AnswerDate { get; set; }
        public virtual Question Question { get; set; }
    }
}