using System;
using System.Collections.Generic;

namespace Application.Dtos
{
    public class AnswerOptionsDto
    {
        public int AnswerId { get; set; }
        public List<int> OptionsId { get; set; }
        public string Description { get; set; }
        public int QuestionId { get; set; }
        public int SolutionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}