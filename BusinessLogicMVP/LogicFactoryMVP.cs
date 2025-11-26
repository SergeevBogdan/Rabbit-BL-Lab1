using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace BusinessLogicMVP
{
    public static class LogicFactoryMVP
    {
        public static LogicMVP CreateLogic(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModuleMVP(useEntityFramework));
            return kernel.Get<LogicMVP>();
        }

        public static ILogicMVP CreateILogic(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModuleMVP(useEntityFramework));
            return kernel.Get<ILogicMVP>();
        }
    }
}
