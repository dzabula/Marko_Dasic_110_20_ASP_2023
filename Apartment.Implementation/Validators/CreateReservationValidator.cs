﻿using Apartment.Application.UseCase.DTO;
using Apartment.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartment.Implementation.Validators
{
    public class CreateReservationValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationValidator(ApartmentContext context)
        {
            RuleFor(x => x.ApartmentId)
                .Cascade(CascadeMode.Stop)
                .Must(x => context.Apartments.Any(y => y.Id == x)).WithMessage("Apartman ne postoji");

            RuleFor(x => new { x.From, x.To })
              .Cascade(CascadeMode.Stop)
              .Must(x => x.From < x.To && x.From > DateTime.UtcNow).WithMessage("Datumi su neispravne vrednosti");

            RuleFor(x => new { x.ApartmentId, x.From, x.To })
                .Cascade(CascadeMode.Stop)
                .Must(x => !context.Reservations.Any() || !context.Reservations.Any(y => y.ApartmentId == x.ApartmentId
                && x.From <= y.To &&
            x.To >= y.From))
                .WithMessage("Apartman je zauzet u trazenom terminu");


        }
    }
}
