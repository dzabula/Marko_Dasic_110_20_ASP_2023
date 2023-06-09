﻿using Apartment.Application.UseCase.Commands.UseCase;
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

        /// <summary>
        /// Dodavanje odredjenih privilegija (IdUseCase) korisnika od strane administratora na platformu - autorizovani korisnici sa odredjenim(IdUseCase => 1) privilegijama.  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>
        ///{
        /// "userId":1,
        /// "useCaseIds":[101]
        /// }
        ///
        ///
        /// </remarks>
        /// <response code="201">Inserted</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut]
        public IActionResult Put([FromBody] AddUseCaseDto obj, [FromServices] IAddUseCaseCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }


    }
}
