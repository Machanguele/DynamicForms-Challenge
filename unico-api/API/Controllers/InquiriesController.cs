using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Inquiries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InquiriesController: BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Inquiry>> CreateInquiry(CreateInquiry.CreateInquiryCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<InquiryDto>>> GetInquiries()
        {
            return await Mediator.Send(new ListInquiries.ListInquiriesQuery());
        } 
        
        [HttpGet("{inquiryId}")]
        public async Task<ActionResult<InquiryDto>> GetQuestionsById(int inquiryId)
        {
            return await Mediator.Send(new GetInquiryById.GetInquiryByIdQuery{InquiryId = inquiryId});
        }
        
        [HttpPut("{inquiryId}")]
        public async Task<ActionResult<Inquiry>> UpdateInquiry(int inquiryId, UpdateInquiry.UpdateInquiryCommand command)
        {
            command.InquiryId = inquiryId;
            return await Mediator.Send(command);
        } 
        
        [HttpPut("submit/{inquiryId}")]
        public async Task<ActionResult<Inquiry>> SubmitInquiry(int inquiryId, SubmitInquiry.SubmitInquiryCommand command)
        {
            command.InquiryId = inquiryId;
            return await Mediator.Send(command);
        }
        
    }
}

