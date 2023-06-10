using Apartment.Application.UseCase.DTO;
using Apartment.Application.UseCase.Queries.Apartment;
using Apartment.DataAccess;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartment.Implementation.UseCase.Queries.Ef.Apartment
{
    public class GetAllReservationQuery : EfBase, IGetAllReservationQuery
    {

        public GetAllReservationQuery(ApartmentContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public int Id => 26;

        public string Name => "Get all reservation - EF";

        public string Description => "";

        public IEnumerable<ReservationDto> Execute(FilterPaginationReservationDto request)
        {
            var res = Context.Reservations.Include(x=>x.User).Include(x=>x.Apartment).AsQueryable();

            if(request.IsPaid != null)
            {
                res = res.Where(x => x.IsPaid);
            }
            
            if(request.ApartmentId > 0)
            {
                res = res.Where(x => x.ApartmentId == request.ApartmentId);
            }

            if(request.CreatedAtFrom != null && request.CreatedAtTo != null && request.CreatedAtTo > request.CreatedAtFrom)
            {
                res = res.Where(x => x.CreatedAt > request.CreatedAtFrom && x.CreatedAt > request.CreatedAtTo);
            }


            if (request.PageNumber == 0 || request.PageSize == 0)
            {
                request.PageNumber = 1;
                request.PageSize = 10;
            }

            var pagination = res.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToList();

            var result = Mapper.Map<IEnumerable<ReservationDto>>(pagination);

            /*   var res2 = new List<ReservationDto>();
               foreach(var x in res)
               {
                   res2.Add(Mapper.Map<IEnumerable<ReservationDto>>(x));
               }
               return res2;*/

            return result;
        }
    }
}
