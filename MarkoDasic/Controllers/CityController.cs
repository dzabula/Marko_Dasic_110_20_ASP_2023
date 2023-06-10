using Apartment.Application.UseCase.Commands.City;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.City;
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
    public class CityController : MyBaseController
    {
        public CityController([FromServices] ApartmentContext context_, UseCaseHandler handler) :base(context_, handler) { }
        
        [HttpGet]
        public IActionResult Get([FromServices] IGetAllCityQuery query)
        {
            var cities = handler.HandleQuery(query, new object());
            return Ok(cities);
        }

        // PUT api/<CityController>/5
        [HttpPut]
        [AllowAnonymous]
        public IActionResult Put([FromBody] CityDto obj, [FromServices] ICreateCityCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }


    }
}
