using System.Collections.Generic;

namespace Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public virtual InputType InputType { get; set; }
        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual Inquiry Inquiry { get; set; }
        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
        
    }
}