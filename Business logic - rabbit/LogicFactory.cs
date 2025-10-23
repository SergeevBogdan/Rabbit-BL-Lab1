using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_logic___rabbit;
using Ninject;
using Ninject.Modules;

namespace Business_logic___rabbit
{
    /// <summary>
    /// Фабрика для создания экземпляров Logic с внедренными зависимостями
    /// </summary>
    public static class LogicFactory
    {
        public static Logic CreateLogic(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<Logic>();
        }
    }
}
