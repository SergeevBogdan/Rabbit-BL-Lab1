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
            // Биндим единый репозиторий
            if (_useEntityFramework)
            {
                Bind<IRepository>().To<EntityRepository>().InSingletonScope();
            }
            else
            {
                Bind<IRepository>().To<DapperRepository>().InSingletonScope();
            }

            // Биндим все сервисы
            Bind<IRabbitAdder>().To<RabbitAdder>().InSingletonScope();
            Bind<IRabbitRemover>().To<RabbitRemover>().InSingletonScope();
            Bind<IRabbitReader>().To<RabbitReader>().InSingletonScope();
            Bind<IRabbitUpdater>().To<RabbitUpdater>().InSingletonScope();
            Bind<IRabbitAgeCalculator>().To<RabbitAgeCalculator>().InSingletonScope();
            Bind<IRabbitWeightCalculator>().To<RabbitWeightCalculator>().InSingletonScope();
            Bind<IRabbitSorter>().To<RabbitSorter>().InSingletonScope();
            Bind<IRabbitRandomCreator>().To<RabbitRandomCreator>().InSingletonScope();
            Bind<IRabbitDisplayer>().To<RabbitDisplayer>().InSingletonScope();
            Bind<IRabbitBreedProvider>().To<RabbitBreedProvider>().InSingletonScope();

            // Биндим композитный сервис
            Bind<Logic>().ToSelf().InSingletonScope();
        }
    }
}

