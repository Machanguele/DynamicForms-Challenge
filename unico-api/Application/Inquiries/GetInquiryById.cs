using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Inquiries
{
    public class GetInquiryById
    {
        public class GetInquiryByIdQuery : IRequest<Inquiry>
        {
            public int InquiryId { get; set; }
        }

        public class Handler : IRequestHandler<GetInquiryByIdQuery, Inquiry>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            
            public async Task<Inquiry> Handle(GetInquiryByIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.Inquiries
                    .Include(x=>x.Questions)
                    .FirstOrDefaultAsync(x=>x.Id == request.InquiryId);
            }
        }
    }
}