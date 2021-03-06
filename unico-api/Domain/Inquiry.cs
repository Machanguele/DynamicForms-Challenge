using System;
using System.Collections.Generic;

namespace Domain
{
    public class Inquiry
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Submitted { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        
    }
}