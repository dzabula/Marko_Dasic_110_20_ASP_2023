using Apartment.Application.UseCase.Commands.Apartment;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Apartment;
using Apartment.DataAccess;
using Apartment.Domain.Entities;
using Apartment.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkoDasic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApartmentController : MyBaseController
    {
        public ApartmentController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

        // GET: api/<ApartmentController>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get([FromQuery] FilterPaginationApartmentDto request, [FromServices] IGetAllApartmentsQuery query)
        {
            var apartments = handler.HandleQuery(query,request);
            return Ok(apartments);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetOneApartmentQuery query)
        {
            var apartment = handler.HandleQuery(query, id);
            return Ok(apartment);
        }


        [HttpPut]
        public IActionResult Put([FromBody] CreateApartmentDto obj, [FromServices] ICreateApartmentCommand createApartmentCommand)
        {
            handler.HandleCommand(createApartmentCommand, obj); 
            return StatusCode(201);
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] UpdateApartmentDto obj, [FromServices] IUpdateApartmentCommand command )
        {
            handler.HandleCommand(command, obj);
            return StatusCode(204);
        }

        // DELETE api/<ApartmentController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteApartmentCommand command)
        {
            handler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
