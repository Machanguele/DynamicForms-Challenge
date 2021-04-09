using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.QuestionOptions
{
    public class ListQuestionCategories
    {
        public class ListQuestionCategoriesQuery : IRequest<List<QuestionCategory>>
        {
            
        }

        public class Handler : IRequestHandler<ListQuestionCategoriesQuery, List<QuestionCategory>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            
            public async Task<List<QuestionCategory>> Handle(ListQuestionCategoriesQuery request, CancellationToken cancellationToken)
            {
                return await _context.QuestionCategories.ToListAsync();
            }
        }
    }
}