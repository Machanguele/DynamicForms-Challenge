using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Questions
{
    public class GetQuestionById
    {
        public class GetQuestionByIdQuery : IRequest<QuestionsDto>
        {
            public int QuestionId { get; set; }
        }

        public class Handler : IRequestHandler<GetQuestionById.GetQuestionByIdQuery, QuestionsDto>
        {
            private readonly DataContext _context;
            private readonly IPhotosUrl _photosUrl;

            public Handler(DataContext context, IPhotosUrl photosUrl)
            {
                _context = context;
                _photosUrl = photosUrl;
            }
            
            
            public async Task<QuestionsDto> Handle(GetQuestionById.GetQuestionByIdQuery request, 
                CancellationToken cancellationToken)
            {
                var question = await _context.Questions
                    .Include(x=>x.Inquiry)
                    .Include(x=>x.QuestionOptions)
                    .Where(x=>x.Id == request.QuestionId)
                    .FirstOrDefaultAsync();

                if (question == null)
                {
                    return null;
                }

                return new QuestionsDto
                {
                    Id = question.Id,
                    Inquiry = question.Inquiry,
                    InputType = question.InputType,
                    QuestionCategory = question.QuestionCategory,
                    Title = question.Title,
                    IsRequired = question.IsRequired,
                    Images = await _photosUrl.GetImagesPath(question.Id)
                };
            }
        }
    }
}