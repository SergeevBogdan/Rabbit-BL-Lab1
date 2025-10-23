using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_logic___rabbit;
using Ninject.Modules;
using RabbitDAL;


namespace Business_logic___rabbit
{
    /// <summary>
    /// Модуль конфигурации Ninject для управления зависимостями
    /// Полностью настраивает все зависимости системы
    /// </summary>
    public class RabbitNinjectModule : NinjectModule
    {
        private readonly bool _useEntityFramework;

        public RabbitNinjectModule(bool useEntityFramework = true)
        {
            _useEntityFramework = useEntityFramework;
        }

        public override void Load()
        {
            if (_useEntityFramework)
            {
                Bind<IRepository>().To<EntityRepository>().InSingletonScope();
            }
            else
            {
                Bind<IRepository>().To<DapperRepository>().InSingletonScope();
            }

            Bind<IRabbitAdder>().To<RabbitAdder>();
            Bind<IRabbitRemover>().To<RabbitRemover>();
            Bind<IRabbitReader>().To<RabbitReader>();
            Bind<IRabbitUpdater>().To<RabbitUpdater>();
            Bind<IRabbitAgeCalculator>().To<RabbitAgeCalculator>();
            Bind<IRabbitWeightCalculator>().To<RabbitWeightCalculator>();
            Bind<IRabbitSorter>().To<RabbitSorter>();
            Bind<IRabbitRandomCreator>().To<RabbitRandomCreator>();
            Bind<IRabbitDisplayer>().To<RabbitDisplayer>();
            Bind<IRabbitBreedProvider>().To<RabbitBreedProvider>();

            Bind<Logic>().ToSelf().InSingletonScope();
        }
    }
}

