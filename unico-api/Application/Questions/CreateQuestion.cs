using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Errors;
using Application.Inquiries;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using SendGrid.Helpers.Errors.Model;

namespace Application.Questions
{
    public class CreateQuestion
    {
             
        public class CreateQuestionCommand : IRequest<Question>
        {
            public string Title { get; set; }
            public bool IsRequired { get; set; }
            public int InputTypeId { get; set; }
            public int QuestionCategoryId { get; set; }
            public int InquiryId { get; set; }
            public List<string> QuestionOptions { get; set; }
            
        }
        
        

        public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestion.CreateQuestionCommand>
        {
            public CreateQuestionCommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.InputTypeId).NotEmpty();
                RuleFor(x => x.QuestionCategoryId).NotEmpty();
                RuleFor(x => x.QuestionCategoryId).NotEmpty();
                RuleFor(x => x.InquiryId).NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<CreateQuestion.CreateQuestionCommand, Question>
        {
            private readonly DataContext _context;
            private readonly IConfiguration _configuration;

            public Handler(DataContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
            
            public async Task<Question> Handle(CreateQuestion.CreateQuestionCommand request, CancellationToken cancellationToken)
            {
                var inputType = await _context.InputTypes
                    .FirstOrDefaultAsync(x => x.Id == request.InputTypeId);
                if (inputType == null)
                    throw new NotFoundException("Input type not found");
            
                var questionCategory = await _context.QuestionCategories
                    .FirstOrDefaultAsync(x => x.Id == request.QuestionCategoryId);
                if (questionCategory == null)
                    throw new NotFoundException("Question Category not not found");
            
                var inquiry = await _context.Inquiries
                    .FirstOrDefaultAsync(x => x.Id == request.InquiryId);
                if (inquiry == null)
                    throw new NotFoundException("Inquiry not not found");
            
                
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var question = new Question
                        {
                            Title = request.Title,
                            Inquiry = inquiry,
                            InputType = inputType,
                            QuestionCategory = questionCategory,
                            IsRequired = request.IsRequired
                        };

                        _context.Questions.Add(question);
                        if (await _context.SaveChangesAsync() < 1)
                        {
                            transaction.Rollback();
                            throw new Exception("Fail while saving Question"); 
                        }
                            
                        
                        if (request.QuestionOptions.Count > 0)
                        {
                            foreach (var option in request.QuestionOptions)
                            {
                                var questionPotion = new QuestionOption
                                {
                                    Question = question,
                                    Description = option

                                };
                                _context.QuestionOptions.Add(questionPotion);
                            }

                            if (await _context.SaveChangesAsync() < 1)
                            {
                                transaction.Rollback();
                                throw new Exception("Fail to save option");
                            }
                        }
                
                        transaction.Commit();
                        return question;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new RestException(HttpStatusCode.NotFound, e.ToString());

                    }
                }
            }
        }
    }
}