using System;
using System.Collections.Generic;

namespace Application.Dtos
{
    public class InquiryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CreationDate { get; set; }
        public bool Submitted { get; set; }
        public List<QuestionsDto> QuestionsDtos { get; set; }
        
    }
}