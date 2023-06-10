using Apartment.Application.UseCase.Commands.Specification;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Specification;
using Apartment.DataAccess;
using Apartment.Domain.Entities;
using Apartment.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkoDasic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SpecificationController : MyBaseController
    {
        public SpecificationController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

     
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get([FromServices] IGetAllSpecificationQuery query)
        {
            var res = handler.HandleQuery(query,new object());
            return Ok( res);
        }

     

        // Dodavanje Specifikacija u veznu tabelu sa apartmanom (ApartmanSpecifications)
        [HttpPost]
        public IActionResult Post([FromBody] ApartmentSpecificationDto obj, [FromServices] ICreateApartmentSpecificatonCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }

        [HttpPut]
        public IActionResult Put([FromBody] SpecificationDto dto, [FromServices] ICreateSpecificationCommand command)
        {
            handler.HandleCommand(command,dto);
            return StatusCode(201);
        }

    }
}
