using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitSharedMVP;

namespace BusinessLogicMVP
{
    public static class ModelFactory
    {
        public static IModel CreateModel(bool useEntityFramework = true)
        {
            var logic = LogicFactoryMVP.CreateILogic(useEntityFramework);
            return new Model(logic);
        }
    }
}
