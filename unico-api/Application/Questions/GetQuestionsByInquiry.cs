using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Inquiries;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Questions
{
    public class GetQuestionsByInquiry
    {
        public class GetQuestionsByInquiryQuery : IRequest<List<QuestionsDto>>
        {
            public int InquiryId { get; set; }
        }

        public class Handler : IRequestHandler<GetQuestionsByInquiry.GetQuestionsByInquiryQuery, List<QuestionsDto>>
        {
            private readonly DataContext _context;
            private readonly IPhotosUrl _photosUrl;

            public Handler(DataContext context, IPhotosUrl photosUrl)
            {
                _context = context;
                _photosUrl = photosUrl;
            }
            
            
            public async Task<List<QuestionsDto>> Handle(GetQuestionsByInquiry.GetQuestionsByInquiryQuery request, 
                CancellationToken cancellationToken)
            {
                var questions =  await _context.Questions
                    .Include(x=>x.Inquiry)
                    .Include(x=>x.QuestionOptions)
                    .Where(x=>x.Inquiry.Id == request.InquiryId)
                    .ToListAsync();
                
                
                var questionsDto = new List<QuestionsDto>();
                foreach (var question in questions)
                {
                    questionsDto.Add(new QuestionsDto
                    {
                        Id = question.Id,
                        Inquiry = question.Inquiry,
                        InputType = question.InputType,
                        QuestionCategory = question.QuestionCategory,
                        Title = question.Title,
                        IsRequired = question.IsRequired,
                        Images = await _photosUrl.GetImagesPath(question.Id) 
                    });
                }

                return questionsDto;
            }
        }
        
    }
}