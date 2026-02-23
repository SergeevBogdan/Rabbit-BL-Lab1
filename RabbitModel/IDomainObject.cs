using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitModel
{
    public interface IDomainObject
    { 
        int Id { get; set; }
    }
}