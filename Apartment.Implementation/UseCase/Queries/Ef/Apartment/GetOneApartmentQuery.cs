using Apartment.Application.Exceptions;
using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Apartment;
using Apartment.DataAccess;
using Apartment.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Apartment.Implementation.UseCase.Queries.Ef.Apartment
{
    public class GetOneApartmentQuery : EfBase, IGetOneApartmentQuery
    {
        public GetOneApartmentQuery(ApartmentContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public int Id => 12;

        public string Name => "Get One Apartment - EF";

        public string Description =>"";

        public ApartmentDto Execute(int request)
        {
            if(!Context.Apartments.Any(x=>x.Id == request)) { throw new EntityNotFoundException("Apartman",request); }
            
            IEnumerable<CommentDto> comments = Context.Comments.Where(x => x.AppartmentId == request && x.ParrentId == null).Select(x=> new CommentDto 
            { 
                Id = x.Id,
                ParrentId = x.ParrentId,
                Text =x.Text,
                User = new UserDto { Id = x.Author.Id, FullName = x.Author.FirstName + " " + x.Author.LastName},
                Childs = null
            
            }).ToList();
           
            
            foreach(var com in comments)
            {
                com.Childs = Context.Comments.Where(t => t.ParrentId == com.Id).Select(y=> new CommentDto
                {
                    Id= y.Id,
                    ParrentId = y.ParrentId,
                    Text = y.Text,
                    User = new UserDto { Id = y.Author.Id, FullName = y.Author.FirstName + " " + y.Author.LastName },
                    Childs = null
                }).ToList();
            }

         //   var x = Context.Apartments.Find(request);
            var x = Context.Apartments
                        .Include(a => a.ApartmentSpecifications)
                        .ThenInclude(s => s.Specification)
                        .FirstOrDefault(a => a.Id == request);


            Context.Entry(x).Reference("Author").Load();
            Context.Entry(x).Reference("CategoryOfApartment").Load();
            Context.Entry(x).Reference("City").Load();
            Context.Entry(x).Reference("Thumb").Load();
            
            Context.Entry(x).Collection("Prices").Load();
            Context.Entry(x).Collection("Rates").Load();
            Context.Entry(x).Collection("Reservations").Load();
            Context.Entry(x).Collection("Comments").Load();
            //Context.Entry(x).Collection("ApartmentSpecifications").Load();
            Context.Entry(x).Collection(x=>x.Images).Load();
    


            return new ApartmentDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                GeoLocationX = x.GeoLocationX,
                GeoLocationY = x.GeoLocationY,
                User = new UserDto { Id = x.Author.Id, FullName = x.Author.FirstName + x.Author.LastName, Email = x.Author.Email },
                Category = new CategoryDto { Id = x.CategoryId, Name = x.CategoryOfApartment.Name },
                City = new CityDto { Id = x.CityId, Name = x.City.Name },
                File = new FileDto { Id = x.FileId, Alt = x.Thumb.Alt, Path = x.Thumb.Path, Extension = x.Thumb.Extension, Size = x.Thumb.Size },
                Price = x.Prices.Where(y => y.ApartmentId == x.Id).OrderByDescending(y => y.CreatedAt).Select(y => new PriceDto
                {
                    Id = y.Id,
                    PricePerNight = y.PricePerNight,
                    PricePerNightWeekend = y.PricePerNightWeekend,
                    PriceOnHoliday = y.PriceOnHoliday

                }).FirstOrDefault(),
                Rates = x.Rates.Select(y => new RateDto
                {
                    Id = y.Id,
                    Value = y.Value
                }),
                Reservations = x.Reservations.Select(y => new ReservationDto
                {
                    Id = y.Id,
                    ApartmentId = request,
                    UserId = x.UserId,
                    IsPaid = y.IsPaid,
                    From = y.From,
                    To = y.To,
                    Amount = y.Amount
                }),
                Comments = comments,
                ApartmentSpecifications = x.ApartmentSpecifications.Select(y => new ApartmentSpecificationDto
                {
                    Id = y.Id,
                    ApartmentId = request,
                    Specification = y.Specification.Name,
                    Value = y.Value,
                    SpecificationId = y.SpecificationId

                }),
                Images = Context.Images.Include(y => y.File).Where(y=>y.ApartmentId == x.Id).Select(y => new FileDto {Id = y.Id, Path = y.File.Path, Alt = "Alternative text" }).ToList()
                

            };
        }
    }
}
