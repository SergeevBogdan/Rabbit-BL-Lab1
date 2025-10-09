﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitModel
{
    public class Rabbit : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; } 
        public int Age { get; set; }
        public int Weight { get; set; }
    }
}