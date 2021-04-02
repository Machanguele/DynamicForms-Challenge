using System.Collections.Generic;

namespace Domain
{
    public class QuestionOption
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual Question Question { get; set; }
        
        
    }
}