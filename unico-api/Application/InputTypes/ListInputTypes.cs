using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.InputTypes
{
    public class ListInputTypes
    {
        public class ListInputTypesQuery : IRequest<List<InputType>>
        {
            
        }

        public class Handler : IRequestHandler<ListInputTypesQuery, List<InputType>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<List<InputType>> Handle(ListInputTypesQuery request, CancellationToken cancellationToken)
            {
                return await _context.InputTypes.ToListAsync();
            }
        }
    }
}