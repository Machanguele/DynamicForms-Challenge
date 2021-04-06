using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Inquiries
{
    public class ListInquiries
    {
        public class ListInquiriesQuery : IRequest<List<Inquiry>>
        {
            
        }

        public class Handler : IRequestHandler<ListInquiriesQuery, List<Inquiry>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            
            public async Task<List<Inquiry>> Handle(ListInquiriesQuery request, CancellationToken cancellationToken)
            {
                return await _context.Inquiries
                    .Include(x=>x.Questions)
                    .ToListAsync();
            }
        }
        
    }
}