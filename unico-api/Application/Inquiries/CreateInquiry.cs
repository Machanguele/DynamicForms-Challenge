using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.Inquiries
{
    public class CreateInquiry
    {
        public class CreateInquiryCommand : IRequest<Inquiry>
        {
            public string Description { get; set; }
        }

        public class CreateInquiryCommandValidator : AbstractValidator<CreateInquiryCommand>
        {
            public CreateInquiryCommandValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<CreateInquiryCommand, Inquiry>
        {
            private readonly DataContext _context;
            private readonly IConfiguration _configuration;

            public Handler(DataContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
            
            public async Task<Inquiry> Handle(CreateInquiryCommand request, CancellationToken cancellationToken)
            {
                var inquiry = new Inquiry
                {
                    Description = request.Description,
                    CreationDate = DateTime.Now
                    
                };

                _context.Inquiries.Add(inquiry);
                if (await _context.SaveChangesAsync() < 1)
                    throw new Exception("Fail while saving Inquiry");
                
                return inquiry;
            }
        }
    }
}