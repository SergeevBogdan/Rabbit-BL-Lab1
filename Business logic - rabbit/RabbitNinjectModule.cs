using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_logic___rabbit;
using Ninject.Modules;
using RabbitDAL;
using RabbitDAL.RabbitDAL;
using RabbitModel;


namespace Business_logic___rabbit
{

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
                    Bind<IReadRepository>().To<EntityRepository>().InSingletonScope();
                    Bind<IWriteRepository>().To<EntityRepository>().InSingletonScope();
                }
                else
                {
                    Bind<IRepository>().To<DapperRepository>().InSingletonScope();
                    Bind<IReadRepository>().To<DapperRepository>().InSingletonScope();
                    Bind<IWriteRepository>().To<DapperRepository>().InSingletonScope();
                }

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
            }
        }
    }
