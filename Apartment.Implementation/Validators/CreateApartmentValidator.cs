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
    public class CreateApartmentValidator : AbstractValidator<CreateApartmentDto>
    {
        public CreateApartmentValidator(ApartmentContext context)
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Naslov je obavezan podatak.")
                .MinimumLength(3).WithMessage("Minimalan broj slova je 3.")
                .Must(x => !context.Apartments.Any(u => u.Title == x)).WithMessage("Naslov {PropertyValue} vec postoji u bazi.");

            RuleFor(x => x.Description)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("Naslov je obavezan podatak.")
               .MinimumLength(3).WithMessage("Minimalan broj slova je 3.");
            RuleFor(x => x.CityId)
               .Cascade(CascadeMode.Stop)
               .Must(x => context.Cities.Any(u => u.Id == x)).WithMessage("Grad sa id:{PropertyValue} Ne postoji u bazi.");
            RuleFor(x => x.CategoryId)
               .Cascade(CascadeMode.Stop)
               .Must(x => context.Categories.Any(u => u.Id == x)).WithMessage("Kategorija sa Id: {PropertyValue} Ne postoji u bazi.");
            RuleFor(x => x.FileId)
               .Cascade(CascadeMode.Stop)
               .Must(x => context.Files.Any(u => u.Id == x)).WithMessage("Fajl sa Id: {PropertyValue} Ne postoji u bazi.");




        
        }
    }
}
