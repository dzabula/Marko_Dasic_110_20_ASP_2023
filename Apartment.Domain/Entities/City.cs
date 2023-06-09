﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apartment.Domain.Entities
{
    public class City : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Apartment>   Apartments{ get; set; }
    }
}
