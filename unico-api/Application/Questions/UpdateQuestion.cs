using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SendGrid.Helpers.Errors.Model;

namespace Application.Questions
{
    public class UpdateQuestion
    {
        public class UpdateQuestionCommand: IRequest<Question>
        {
            public int QuestionId { get; set; }
            public string Title { get; set; }
            public bool IsRequired { get; set; }
            public int InputTypeId { get; set; }
            public int QuestionCategoryId { get; set; }
        }
        
        public class UpdateQuestionCommandValidator: AbstractValidator<UpdateQuestionCommand>
        {
        }
        
        public class UpdateQuestionCommandHandler: IRequestHandler<UpdateQuestionCommand, Question>
        {
            private readonly DataContext _context;
            
            public UpdateQuestionCommandHandler(DataContext context)
            {
                _context = context;
                
            }
            
            public async Task<Question> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
            {

                var question = await _context.Questions
                    .Include(x => x.Inquiry)
                    .Include(x => x.InputType)
                    .Include(x => x.QuestionCategory)
                    .FirstOrDefaultAsync(x => x.Id == request.QuestionId);
                if (question == null)
                    throw new NotFoundException("Question Not Found");
                
                var inputType = await _context.InputTypes
                    .FirstOrDefaultAsync(x => x.Id == request.InputTypeId);
                if (inputType == null)
                    throw new NotFoundException("Input type not found");
            
                var questionCategory = await _context.QuestionCategories
                    .FirstOrDefaultAsync(x => x.Id == request.QuestionCategoryId);
                if (questionCategory == null)
                    throw new NotFoundException("Question Category not found");
                
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        question.Title = request.Title ?? request.Title;
                        question.InputType = inputType;
                        question.IsRequired = request.IsRequired;
                        question.QuestionCategory = questionCategory;

                        _context.Entry(question).State = EntityState.Modified;
                        if (await _context.SaveChangesAsync() >1)
                        {
                            transaction.Rollback();
                            throw new Exception("Fail while editing Question"); 
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