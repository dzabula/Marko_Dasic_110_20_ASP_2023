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

        /// <summary>
        /// Preglde svih rezervacija apartmna Uz omoguceno filtriranje i paginaciju - autorizovani korisnici sa odredjenim (IdUseCase => 26) privilegijama.  
        /// </summary>
        /// <param name="request"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Moguce je filtrirati pretragu rezervacija po datumu, Id apartmana, da li je placeno. Takodje s unosom broja zeljenih stranica za brikaz kao i velicine tih stranica relaizuje paginacija
        ///  
        /// CreatedAtFrom predstavlja datum od kada rezervacija pocinje,  CreatedAtTo predstavlja do kada je apartman rezervisan. 
        /// 
        /// CreatedAtFrom => "2000-05-11T21:13:31.594Z"
        /// CreatedAtTo => "2030-05-11T21:13:31.594Z"
        /// isPaid = false
        /// 
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        public IActionResult Get([FromQuery] FilterPaginationReservationDto obj,[FromServices] IGetAllReservationQuery query)
        {
            var res = handler.HandleQuery(query, obj);
            return Ok(res);
        }
        /// <summary>
        /// Dodavanje rezervacije apartmana - autorizovani korisnici sa odredjenim(IdUseCase => 17) privilegijama.  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>  
        ///     Korinsik kreira rezervaciju unosenjem Id apartmana, datumo od, do. Ukoliko je apartman zauzet nekim danima koje spadaju u opseg u kom je korisnik zeleo da ga rezervise, bice obavesten porukom da je termin zauzet.
        ///     
        /// 
        /// {
        ///     "apartmentId":1,
        ///     "from":"2025-05-11T21:13:31.594Z",
        ///     "to":"2025-06-11T21:13:31.594Z"
        /// 
        /// }
        /// 
    /// </remarks>
    /// <response code="201">Inserted</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Unprocessable Entity</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpPut]
        public IActionResult Put([FromBody] CreateReservationDto obj, [FromServices] ICreateReservationsCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(201);
        }



        /// <summary>
        /// Brisanje Rezervacija od strane admina - autorizovani korisnici sa odredjenim (IdUseCase => 28) privilegijama.  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns return="ApartmentDto"></returns>
        /// <remarks>
        ///     Aministrator moze brisati sve rezervacije (soft delete).
        ///     proslediti id => 16 kako biste sigurno obrisali jednu rezervacju
        /// </remarks>
        /// <response code="204">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="409">Conflict</response>
        /// <response code="404">Not Found Entity</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteReservationCommand command)
        {
            handler.HandleCommand(command,id);
            return NoContent();
        }

      


        
    }
}
