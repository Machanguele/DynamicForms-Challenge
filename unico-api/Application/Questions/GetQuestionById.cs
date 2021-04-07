using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Questions
{
    public class GetQuestionById
    {
        public class GetQuestionByIdQuery : IRequest<Question>
        {
            public int QuestionId { get; set; }
        }

        public class Handler : IRequestHandler<GetQuestionById.GetQuestionByIdQuery, Question>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            
            public async Task<Question> Handle(GetQuestionById.GetQuestionByIdQuery request, 
                CancellationToken cancellationToken)
            {
                return await _context.Questions
                    .Include(x=>x.Inquiry)
                    .Include(x=>x.QuestionOptions)
                    .Where(x=>x.Id == request.QuestionId)
                    .FirstOrDefaultAsync();
            }
        }
    }
}