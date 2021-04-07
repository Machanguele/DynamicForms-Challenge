using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Errors;
using Application.Inquiries;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Persistence;
using SendGrid.Helpers.Errors.Model;

namespace Application.Questions
{
    public class CreateQuestion
    {
             
        public class CreateQuestionCommand : IRequest<QuestionsDto>
        {
            public string Title { get; set; }
            public bool IsRequired { get; set; }
            public int InputTypeId { get; set; }
            public int QuestionCategoryId { get; set; }
            public int InquiryId { get; set; }
            public IList<IFormFile> Files { get; set; }
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
        
        public static string GenerateSecureString(int bytes)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var tokenData = new byte[bytes];
                rng.GetBytes(tokenData);

                return MakeBase64UrlSafe(Convert.ToBase64String(tokenData));
            }
        }
        
        public static string MakeBase64UrlSafe(string input)
        {
            return input.TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        
        public class Handler : IRequestHandler<CreateQuestion.CreateQuestionCommand, QuestionsDto>
        {
            private readonly DataContext _context;
            private readonly IConfiguration _configuration;
            private readonly IHostEnvironment _environment;
            private readonly IPhotosUrl _photosUrl;

            public Handler(DataContext context,
                IConfiguration configuration,
                IHostEnvironment environment,
                IPhotosUrl photosUrl
                )
            {
                _context = context;
                _configuration = configuration;
                _environment = environment;
                _photosUrl = photosUrl;
            }
            
            public async Task<QuestionsDto> Handle(CreateQuestion.CreateQuestionCommand request, CancellationToken cancellationToken)
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
                        
                        foreach (var file in request.Files)
                        {
                            var uploadDir = _configuration["UploadDir"];
                            var root = "/";
                            if (uploadDir[0] != '/') root = _environment.ContentRootPath;
                            var finalUploadDir = Path.Combine(root, uploadDir);
                            var ext = Path.GetExtension(file.FileName);
                            var fileToken = $"{GenerateSecureString(20)}{ext}";

                            if (file.Length > 0)
                            {
                                using (var fileStream = new FileStream(Path.Combine(finalUploadDir, fileToken),
                                    FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);

                                    var image = new Image
                                    {
                                        Name = file.FileName,
                                        Url = fileToken,
                                        Question = question
                                    };

                                    _context.Images.Add(image);
                                    
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                
                        transaction.Commit();
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