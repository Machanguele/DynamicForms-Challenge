using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SendGrid.Helpers.Errors.Model;

namespace Application.Inquiries
{
    public class UpdateInquiry
    {
        public class UpdateInquiryCommand: IRequest<Inquiry>
        {
            public int InquiryId { get; set; }
            public string Description { get; set; }
        }
        
        public class UpdateInquiryCommandValidator: AbstractValidator<UpdateInquiryCommand>
        {
        }
        
        public class UpdateInquiryCommandHandler: IRequestHandler<UpdateInquiryCommand, Inquiry>
        {
            private readonly DataContext _context;
            
            public UpdateInquiryCommandHandler(DataContext context)
            {
                _context = context;
                
            }
            
            public async Task<Inquiry> Handle(UpdateInquiryCommand request, CancellationToken cancellationToken)
            {

                var inquiry = await _context.Inquiries
                    .Include(x=>x.Questions)
                    .FirstOrDefaultAsync(x => x.Id == request.InquiryId);
                if (inquiry == null)
                    throw new NotFoundException("Inquiry Not Found");
                
                inquiry.Description = request.Description;
                

                _context.Entry(inquiry).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() >1)
                {
                    throw new Exception("Fail while editing Inquiry"); 
                }
                return inquiry;
            }
        }
    }
}