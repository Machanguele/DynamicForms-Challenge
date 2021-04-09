using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Inquiries
{
    public class ListInquiries
    {
        public class ListInquiriesQuery : IRequest<List<InquiryDto>>
        {
            
        }

        public class Handler : IRequestHandler<ListInquiriesQuery, List<InquiryDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            
            public async Task<List<InquiryDto>> Handle(ListInquiriesQuery request, CancellationToken cancellationToken)
            {
                var inquiries =  await _context.Inquiries
                    .Include(x=>x.Questions)
                    .OrderBy(x=>x.CreationDate)
                    .ToListAsync();

                var listToReturn = new List<InquiryDto>();
                foreach (var inquiry in inquiries)
                {
                    listToReturn.Add(new InquiryDto
                    {
                        Id = inquiry.Id,
                        Description = inquiry.Description,
                        Submitted = inquiry.Submitted,
                        CreationDate = inquiry.CreationDate.Day +"-"+inquiry.CreationDate.Month + "-"+ inquiry.CreationDate.Year,
                    });
                }

                return listToReturn;
            }
        }
        
    }
}