using Apartment.Application.UseCase.Commands.Comment;
using Apartment.Application.UseCase.DTO;
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
    public class CommentController : MyBaseController
    {
        public CommentController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

       
        [HttpPut]
        public IActionResult Put([FromBody] CreateCommentDto obj, [FromServices] ICreateCommentCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }

        
        [HttpDelete("DeleteYourSelf/{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCommentYourSelfCommand command)
        {
            handler.HandleCommand(command, id);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCommentCommand command)
        {
            handler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
