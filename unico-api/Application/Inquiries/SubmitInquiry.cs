using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SendGrid.Helpers.Errors.Model;

namespace Application.Inquiries
{
    public class SubmitInquiry
    {
        public class SubmitInquiryCommand: IRequest<Inquiry>
        {
            public int InquiryId { get; set; }
        }
        
        public class SubmitInquiryCommandValidator: AbstractValidator<SubmitInquiryCommand>
        {
        }
        
        public class SubmitInquiryCommandHandler: IRequestHandler<SubmitInquiryCommand, Inquiry>
        {
            private readonly DataContext _context;
            
            public SubmitInquiryCommandHandler(DataContext context)
            {
                _context = context;
                
            }
            
            public async Task<Inquiry> Handle(SubmitInquiryCommand request, CancellationToken cancellationToken)
            {
                var inquiry = await _context.Inquiries
                    .Include(x=>x.Questions)
                    .FirstOrDefaultAsync(x => x.Id == request.InquiryId);
                if (inquiry == null)
                    throw new NotFoundException("Inquiry Not Found");
                
                inquiry.Submitted = true;
                _context.Entry(inquiry).State = EntityState.Modified;
                
                if (await _context.SaveChangesAsync() >1)
                {
                    throw new Exception("Fail while e submitting Inquiry"); 
                }
                return inquiry;
            }
        }
    }
}