using System.Threading.Tasks;
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
        
    }
}

