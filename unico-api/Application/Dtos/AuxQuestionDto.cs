using System.Collections.Generic;

namespace Application.Dtos
{
    public class AuxQuestionDto
    {
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public int InputTypeId { get; set; }
        public int QuestionCategoryId { get; set; }
        public int InquiryId { get; set; }
        public List<string> QuestionOptions { get; set; }
    }
}
