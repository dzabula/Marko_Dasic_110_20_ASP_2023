using Apartment.Application.UseCase.Commands.Report;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Report;
using Apartment.DataAccess;
using Apartment.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.ObjectiveC;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkoDasic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportController : MyBaseController
    {
        public ReportController(ApartmentContext context, UseCaseHandler handler) : base(context, handler)
        {
        }

        [HttpGet]
        public IActionResult Get([FromServices] IGetAllReportQuery query)
        {
            var res = handler.HandleQuery(query, new object());
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpGet("/lines")]
        public IActionResult Get1([FromServices] IGetAllReportLineQuery query)
        {
            var res =handler.HandleQuery(query, new object());
            return Ok(res);
            
        }


        // Dodavanje jedne stavke reporta (Admin)
        [HttpPost]
        public IActionResult Post([FromBody] CreateReportLineDto obj, [FromServices] ICreateReportLineCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }

        // Dodavanje reporta od strane korisnika ka apartmanu
        [HttpPut]
        public IActionResult Put([FromBody] CreateReportDto obj, ICreateReportCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        
        }


    }
}
