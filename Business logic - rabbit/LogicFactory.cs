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
    public static class LogicFactory
    {
        public static IRabbitAdder CreateRabbitAdder(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitAdder>();
        }

        public static IRabbitRemover CreateRabbitRemover(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitRemover>();
        }

        public static IRabbitReader CreateRabbitReader(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitReader>();
        }

        public static IRabbitUpdater CreateRabbitUpdater(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitUpdater>();
        }

        public static IRabbitAgeCalculator CreateRabbitAgeCalculator(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitAgeCalculator>();
        }

        public static IRabbitWeightCalculator CreateRabbitWeightCalculator(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitWeightCalculator>();
        }

        public static IRabbitSorter CreateRabbitSorter(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitSorter>();
        }

        public static IRabbitRandomCreator CreateRabbitRandomCreator(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitRandomCreator>();
        }

        public static IRabbitDisplayer CreateRabbitDisplayer(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitDisplayer>();
        }

        public static IRabbitBreedProvider CreateRabbitBreedProvider(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<IRabbitBreedProvider>();
        }
    }
}
