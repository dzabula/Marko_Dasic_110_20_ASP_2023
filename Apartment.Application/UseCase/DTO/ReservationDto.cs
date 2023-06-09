﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartment.Application.UseCase.DTO
{
    public class ReservationDto : BaseDto
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public int ApartmentId { get; set; }
        public int UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
    }

    public class CreateReservationDto
    {
        public int ApartmentId { get; set; }
        public int UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }


    public class FilterPaginationReservationDto
    {
        public DateTime? CreatedAtFrom { get; set; }
        public DateTime? CreatedAtTo { get; set; }
        public int ApartmentId { get; set; }
        public bool? IsPaid { get; set; }    
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
