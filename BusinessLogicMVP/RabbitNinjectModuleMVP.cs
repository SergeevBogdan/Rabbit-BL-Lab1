using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RabbitDAL;

namespace BusinessLogicMVP
{
    public class RabbitNinjectModuleMVP : NinjectModule
    {
        private readonly bool _useEntityFramework;

        public RabbitNinjectModuleMVP(bool useEntityFramework = true)
        {
            _useEntityFramework = useEntityFramework;
        }

        public override void Load()
        {
            ConfigureDataAccess();
            ConfigureBusinessServices();
            ConfigureMainLogic();
        }

        private void ConfigureDataAccess()
        {
            if (_useEntityFramework)
            {
                Bind<IRepository>().To<EntityRepository>().InSingletonScope();
                Bind<RabbitDbContext>().ToSelf().InSingletonScope();
            }
            else
            {
                Bind<IRepository>().To<DapperRepository>().InSingletonScope();
            }
        }

        private void ConfigureBusinessServices()
        {
            Bind<IRabbitAdderMVP>().To<RabbitAdderMVP>();
            Bind<IRabbitRemoverMVP>().To<RabbitRemoverMVP>();
            Bind<IRabbitReaderMVP>().To<RabbitReaderMVP>();
            Bind<IRabbitUpdaterMVP>().To<RabbitUpdaterMVP>();
            Bind<IRabbitAgeCalculatorMVP>().To<RabbitAgeCalculatorMVP>();
            Bind<IRabbitWeightCalculatorMVP>().To<RabbitWeightCalculatorMVP>();
            Bind<IRabbitSorterMVP>().To<RabbitSorterMVP>();
            Bind<IRabbitRandomCreatorMVP>().To<RabbitRandomCreatorMVP>();
            Bind<IRabbitDisplayerMVP>().To<RabbitDisplayerMVP>();
            Bind<IRabbitBreedProviderMVP>().To<RabbitBreedProviderMVP>();
            Bind<IRabbitProviderMVP>().To<RabbitProviderMVP>();
        }

        private void ConfigureMainLogic()
        {
            Bind<ILogicMVP>().To<LogicMVP>().InSingletonScope();
        }
    }
}
