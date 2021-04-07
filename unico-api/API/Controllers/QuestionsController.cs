using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Inquiries;
using Application.Questions;
using BrunoZell.ModelBinding;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OTCApi.Common;
using Persistence;

namespace API.Controllers
{
    public class QuestionsController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IHostEnvironment _environment;

        public QuestionsController(IConfiguration configuration, DataContext context, IHostEnvironment environment)
        {
            _configuration = configuration;
            _context = context;
            _environment = environment;
        }
        
        /*[HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion(CreateQuestion.CreateQuestionCommand command)
        {
            return await Mediator.Send(command);
        }*/
        
        [HttpPost]
        public async Task<ActionResult<QuestionsDto>> CreateQuestion(
            [ModelBinder(BinderType = typeof(JsonModelBinder))]
            CreateQuestion.CreateQuestionCommand command,
            [FromForm] IList<IFormFile> files)
        {
            command.Files = files;
            return await Mediator.Send(command);
        }
       
        
        [HttpGet("ByInquiry/{inquiryId}")]
        public async Task<ActionResult<List<QuestionsDto>>> GetQuestionsByInquiry(int inquiryId)
        {
            return await Mediator.Send(new GetQuestionsByInquiry.GetQuestionsByInquiryQuery{InquiryId = inquiryId});
        }
        
        [HttpGet("{questionId}")]
        public async Task<ActionResult<QuestionsDto>> GetQuestionsById(int questionId)
        {
            return await Mediator.Send(new GetQuestionById.GetQuestionByIdQuery{QuestionId = questionId});
        }
        
        [HttpGet("{questionId}/document/{token}")]
        public async Task<IActionResult> GetFile([FromRoute] int questionId, [FromRoute] string token)
        {
            var document = await _context.Images
                .Where(w => w.Url == token && w.Question.Id == questionId)
                .FirstOrDefaultAsync();

            if (document == null) return BadRequest(new {error = "Unknown document. Probably the token is invalid."});

            var uploadLocalDir = _configuration["UploadDir"];
            var root = "/";

            if (uploadLocalDir[0] != '/') root = _environment.ContentRootPath;

            var finalUploadDir = Path.Combine(root, uploadLocalDir);
            var filePath = $"{finalUploadDir}/{token}";

            var ext = token.Substring(token.Length - 3);
            var mime = MimeTypeMap.GetMimeType(ext);


            if (System.IO.File.Exists(filePath))
                return PhysicalFile($"{finalUploadDir}/{token}", mime);


            return NotFound(new {error = "File does not exist"});
        }
        
        [HttpPut("{questionId}")]
        public async Task<ActionResult<QuestionsDto>> UpdateQuestion(int questionId, UpdateQuestion.UpdateQuestionCommand command)
        {
            command.QuestionId = questionId;

            return await Mediator.Send(command);
        }
        
    }
}