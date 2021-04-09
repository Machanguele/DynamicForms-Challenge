using System.Collections.Generic;
using System.Threading.Tasks;
using Application.InputTypes;
using Application.Inquiries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InputTypesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<InputType>>> GetInputTypes()
        {
            return await Mediator.Send(new ListInputTypes.ListInputTypesQuery());
        } 
    }
}