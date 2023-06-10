using Apartment.Application.UseCase.Commands.Apartment;
using Apartment.Application.UseCase.Commands.Rate;
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
    public class RateController : MyBaseController
    {
        public RateController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

        // PUT api/<RateControllerR>/5
        [HttpPut]
        public IActionResult Put([FromBody] CreateRateDto obj, [FromServices] ICreateRateCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }


    }
}
