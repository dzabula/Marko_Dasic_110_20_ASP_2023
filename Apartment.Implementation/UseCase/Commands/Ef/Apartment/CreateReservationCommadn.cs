using Apartment.Application.Exceptions;
using Apartment.Application.UseCase.Commands.Apartment;
using Apartment.Application.UseCase.DTO;
using Apartment.DataAccess;
using Apartment.Domain;
using Apartment.Implementation.Validators;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartment.Implementation.UseCase.Commands.Ef.Apartment
{
    public class CreateReservationCommadn : EfBase, ICreateReservationsCommand
    {
        private IUser user;
        private CreateReservationValidator validator;
        public CreateReservationCommadn(CreateReservationValidator validator, IUser user,ApartmentContext context, IMapper mapper) : base(context, mapper)
        {
            this.user = user;
            this.validator = validator;
        }

        public int Id => 17;

        public string Name => "Create Reservation - EF";

        public string Description => "";

        public void Execute(CreateReservationDto request)
        {
            if (request is null) throw new BadRequestException();
            request.UserId = user.Id;
            validator.ValidateAndThrow(request);

            var newReservation = new Domain.Entities.Reservation
            {
                ApartmentId = request.ApartmentId,
                UserId = request.UserId,
                From = request.From,
                To = request.To

            };

            Context.Reservations.Add(newReservation);
            Context.SaveChanges();
            
        }
    }
}
