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

namespace Application.Inquiries
{
    public class GetInquiryById
    {
        public class GetInquiryByIdQuery : IRequest<InquiryDto>
        {
            public int InquiryId { get; set; }
        }

        public class Handler : IRequestHandler<GetInquiryByIdQuery, InquiryDto>
        {
            private readonly DataContext _context;
            private readonly IPhotosUrl _photosUrl;

            public Handler(DataContext context, IPhotosUrl photosUrl)
            {
                _context = context;
                _photosUrl = photosUrl;
            }
            
            
            public async Task<InquiryDto> Handle(GetInquiryByIdQuery request, CancellationToken cancellationToken)
            {
                var inquiry = await _context.Inquiries
                    .Include(x=>x.Questions)
                    .FirstOrDefaultAsync(x=>x.Id == request.InquiryId);
                
                var questions = await _context.Questions
                    .Include(x=>x.InputType)
                    .Include(x=>x.QuestionCategory)
                    .Include(x=>x.Inquiry)
                    .Include(x=>x.QuestionOptions)
                    .Where(x=>x.Inquiry.Id == inquiry.Id)
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
                        QuestionOptions = question.QuestionOptions.ToList(),
                        Title = question.Title,
                        IsRequired = question.IsRequired,
                        Images = await _photosUrl.GetImagesPath(question.Id)
                    });
                }

                return new InquiryDto
                {
                    Id = inquiry.Id,
                    Description = inquiry.Description,
                    Submitted = inquiry.Submitted,
                    CreationDate = inquiry.CreationDate.Day +"-"+inquiry.CreationDate.Month + "-"+ inquiry.CreationDate.Year,
                    QuestionsDtos = questionsDto
                };

            }
        }
    }
}