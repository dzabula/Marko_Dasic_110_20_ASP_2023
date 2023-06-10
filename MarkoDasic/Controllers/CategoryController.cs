using Apartment.Application.UseCase.Commands.Category;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Category;
using Apartment.DataAccess;
using Apartment.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkoDasic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoryController : MyBaseController
    {
        public CategoryController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get([FromServices] IGetAllCategoriesQuery query)
        {
            var res = handler.HandleQuery(query, new object());
            return Ok(res);
        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        public IActionResult Put([FromBody] CreateCategoryDto obj, [FromServices] ICreateCategoryCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }


    }
}
