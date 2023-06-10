using Apartment.Application.UseCase.Commands.UseCase;
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
    public class UserUseCase : MyBaseController
    {
        public UserUseCase(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

        [HttpPut]
        public IActionResult Put([FromBody] AddUseCaseDto dto, [FromServices] IAddUseCaseCommand command)
        {
            handler.HandleCommand(command, dto);
            return StatusCode(201);
        }


    }
}
