﻿using Apartment.Application.UseCase.Commands.City;
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
        
        /// <summary>
        /// Pregled sbih gradova - neautorizovani i autorizovani korisnici mogu pristupiti.  
        /// </summary>
        /// <param name="query"></param>
        /// <returns return="IEnumerable<CategoryDto>"></returns>
        /// <remarks>
        ///     Dohvatanje svih gradova. 
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [AllowAnonymous]

        public IActionResult Get([FromServices] IGetAllCityQuery query)
        {
            var cities = handler.HandleQuery(query, new object());
            return Ok(cities);
        }

        /// <summary>
        /// Dodavanje grad - autorizovani korisnici sa odredjenim (IdUseCase => 7) privilegijama.  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="command"></param>
        /// <returns return="ApartmentDto"></returns>
        /// <remarks>
        ///    Administratori mogu dodavati gradove.
        ///    
        /// {
        ///     "name":"Kragujevac"
        /// }
        /// </remarks>
        /// <response code="201">Inserted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut]
        public IActionResult Put([FromBody] CityDto obj, [FromServices] ICreateCityCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }


    }
}
