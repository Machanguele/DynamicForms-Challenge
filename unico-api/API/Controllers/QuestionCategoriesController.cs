using System.Collections.Generic;
using System.Threading.Tasks;
using Application.InputTypes;
using Application.QuestionOptions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class QuestionCategoriesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<QuestionCategory>>> GetQuestionCategories()
        {
            return await Mediator.Send(new ListQuestionCategories.ListQuestionCategoriesQuery());
        } 
    }
}