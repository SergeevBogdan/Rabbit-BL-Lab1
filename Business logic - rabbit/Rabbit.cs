using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
    public class Rabbit: IDomainObject
    {
        public int Id { get; set; }
        public string Name;
        public string Breed;
        public int Age;
        public int Weight;
    }
}
 