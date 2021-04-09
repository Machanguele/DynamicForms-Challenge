using System.Collections.Generic;
using Domain;

namespace Application.Dtos
{
    public class QuestionsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public virtual InputType InputType { get; set; }  
        public virtual List<QuestionOption> QuestionOptions { get; set; }
        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual Inquiry Inquiry { get; set; }
        public List<object> Images { get; set; }
    }
}