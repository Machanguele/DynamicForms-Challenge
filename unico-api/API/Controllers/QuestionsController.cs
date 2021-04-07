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
        
        [HttpGet("{questionId}")]
        public async Task<ActionResult<Question>> GetQuestionsById(int questionId)
        {
            return await Mediator.Send(new GetQuestionById.GetQuestionByIdQuery{QuestionId = questionId});
        }
        
        [HttpPut("{questionId}")]
        public async Task<ActionResult<Question>> UpdateQuestion(int questionId, UpdateQuestion.UpdateQuestionCommand command)
        {
            command.QuestionId = questionId;

            return await Mediator.Send(command);
        }
        
    }
}