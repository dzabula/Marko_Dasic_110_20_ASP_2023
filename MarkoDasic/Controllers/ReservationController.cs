using Apartment.Application.UseCase.Commands.Apartment;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Apartment;
using Apartment.DataAccess;
using Apartment.Implementation;
using Apartment.Implementation.UseCase.Commands.Ef.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkoDasic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReservationController : MyBaseController
    {
        public ReservationController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

        
        [HttpGet]
        public IActionResult Get([FromQuery] FilterPaginationReservationDto obj,[FromServices] IGetAllReservationQuery query)
        {
            var res = handler.HandleQuery(query, obj);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPut]
        public IActionResult Put([FromBody] CreateReservationDto obj, [FromServices] ICreateReservationsCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteReservationCommand command)
        {
            handler.HandleCommand(command,id);
            return NoContent();
        }

      


        
    }
}
