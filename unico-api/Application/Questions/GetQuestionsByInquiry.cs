using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Inquiries;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Questions
{
    public class GetQuestionsByInquiry
    {
        public class GetQuestionsByInquiryQuery : IRequest<List<Question>>
        {
            public int InquiryId { get; set; }
        }

        public class Handler : IRequestHandler<GetQuestionsByInquiry.GetQuestionsByInquiryQuery, List<Question>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            
            public async Task<List<Question>> Handle(GetQuestionsByInquiry.GetQuestionsByInquiryQuery request, 
                CancellationToken cancellationToken)
            {
                return await _context.Questions
                    .Include(x=>x.Inquiry)
                    .Include(x=>x.QuestionOptions)
                    .Where(x=>x.Inquiry.Id == request.InquiryId)
                    .ToListAsync();
            }
        }
        
    }
}