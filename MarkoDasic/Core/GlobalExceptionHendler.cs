
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Apartment.Application;
using Apartment.Application.Logging;
using Apartment.Application.Exceptions;
using System;
using Microsoft.EntityFrameworkCore;
using Apartment.DataAccess;

namespace ASPNedelja3Vezbe.Api.Core
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLoger logger;
        private readonly ApartmentContext context;
  

        public GlobalExceptionHandler(ApartmentContext context,RequestDelegate next, IExceptionLoger logger)
        {
            _next = next;
            this.logger = logger;
            this.context = context;
    
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
               
                context.Database.ExecuteSqlRaw("SAVE TRANSACTION MySavePoint");
                try
                {
                    await _next(httpContext);
                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    logger.Log(ex);
                    transaction.RollbackToSavepoint("MySavePoint");

                    httpContext.Response.ContentType = "application/json";
                    object response = null;
                    var statusCode = StatusCodes.Status500InternalServerError;


                    if (ex is ForbiddenUseCaseExec)
                    {
                        statusCode = StatusCodes.Status403Forbidden;
                        response = new object();
                    }
                    if (ex is WrongFileExtensionsException)
                    {
                        statusCode = StatusCodes.Status400BadRequest;
                        response = new { error = ex.Message };
                    }

                    if (ex is EntityNotFoundException)
                    {
                        statusCode = StatusCodes.Status404NotFound;
                        response = new object();

                    }
                    if (ex is UnauthorizedAccessException)
                    {
                        statusCode = StatusCodes.Status401Unauthorized;
                        response = new object();

                    }
                    if (ex is BadRequestException)
                    {
                        statusCode = StatusCodes.Status400BadRequest;
                        response = new object();

                    }
                    if (ex is FluentValidation.ValidationException e)
                    {
                        statusCode = StatusCodes.Status422UnprocessableEntity;
                        response = new
                        {

                            errors = e.Errors.Select(x => new
                            {
                                property = x.PropertyName,
                                error = x.ErrorMessage
                            })
                        };
                    }
                    if (response is null)
                    {
                        response = new
                        {
                            Message = "Doslo je do grseke na severu pokusajte ponovo. Greska je:" + ex.Message
                        };
                    }
                    /*
                    if (ex is UseCaseConflictException conflictEx)
                    {
                        statusCode = StatusCodes.Status409Conflict;
                        response = new { message = conflictEx.Message };
                    }
                    */

                    httpContext.Response.StatusCode = statusCode;
                    await httpContext.Response.WriteAsJsonAsync(response);
                    
                }
            }
        }
        
    }
}
