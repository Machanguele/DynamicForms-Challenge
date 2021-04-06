using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Inquiries;
using Application.Questions;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class QuestionsController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion(CreateQuestion.CreateQuestionCommand command)
        {
            return await Mediator.Send(command);
        }
       
        
        [HttpGet("ByInquiry/{inquiryId}")]
        public async Task<ActionResult<List<Question>>> GetQuestionsByInquiry(int inquiryId)
        {
            return await Mediator.Send(new GetQuestionsByInquiry.GetQuestionsByInquiryQuery{InquiryId = inquiryId});
        }
        
    }
}