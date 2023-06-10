using Apartment.Application.UseCase.Commands.User;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.User;
using Apartment.DataAccess;
using Apartment.Domain;
using Apartment.Domain.Entities;
using Apartment.Implementation;
using Apartment.Implementation.UseCase.Commands.Ef.User;
using Apartment.Implementation.UseCase.Queries.Ef.User;
using MarkoDasic.Core;
using MarkoDasic.Core.TokenStorage;
using MarkoDasic.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarkoDasic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : MyBaseController
    {

        public UserController([FromServices] ApartmentContext context, [FromServices] UseCaseHandler handler) : base(context, handler)
        {
            
        }


        [HttpGet]
        public IActionResult Get([FromQuery] FilterPaginationUserDto obj, [FromServices] IGetAllUsersQuery query)
        {
        
            var all = handler.HandleQuery(query,obj);
            return Ok(all);
        }

        [HttpGet("GetYourSelf")]
        public IActionResult GetYourSelf([FromServices] GetYourSelfQuery query, [FromServices] IUser user)
        {
            var id = user.Id;
            var all = handler.HandleQuery(query, id);
            return Ok(all);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetOneUserQuery query)
        {
            var user = handler.HandleQuery(query, id);
            return Ok(user);
        }

        //LoginUsera
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] LogInUserDto obj, [FromServices] JwtManager manager)
        {
            var token = manager.MakeToken(obj.Email, obj.Password);
            return Ok(token);
            
        }


        //Registracija usera
        [AllowAnonymous]
        [HttpPut]
        public IActionResult Put([FromBody] CreateUserDto obj, [FromServices] ICreateUserCommand createUserCommand)
        {
            handler.HandleCommand(createUserCommand, obj);
            return StatusCode(201);
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] UpdateUserDto obj, [FromServices] IUpdateUserCommand command)
        {
            handler.HandleCommand(command, obj);
            return StatusCode(204);
        }

        [HttpPatch("UpdateYourSelf")]
        public IActionResult PatchYourSelf([FromBody] UpdateUserDto obj, [FromServices] IUser user, [FromServices] UpdateYourSelfCommand command)
        {
            obj.Id = user.Id;
            handler.HandleCommand(command, obj);
            return StatusCode(204);
        }
        

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserCommand command)
        {
            handler.HandleCommand(command, id);
            return NoContent();
        }

        [HttpDelete("DeleteYourSelf/{id}")]
        public IActionResult DeleteYourSelf([FromServices] IDeleteYourSelfCommand command, [FromServices] ITokenStorage token)
        {
            handler.HandleCommand(command,new object());
            this.InvalidateToken(token);
            return NoContent();
        }


        [HttpDelete("logout")]
        public IActionResult InvalidateToken([FromServices] ITokenStorage storage)
        {
            var header = HttpContext.Request.Headers["Authorization"];

            var token = header.ToString().Split("Bearer ")[1];

            var handler = new JwtSecurityTokenHandler();

            var tokenObj = handler.ReadJwtToken(token);

            string jti = tokenObj.Claims.FirstOrDefault(x => x.Type == "jti").Value;

            storage.InvalidateToken(jti);

            return NoContent();
        }
    }
}
