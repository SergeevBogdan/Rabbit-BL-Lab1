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
    /// Конфигурационный модуль Ninject для управления зависимостями приложения
    /// Реализует инверсию управления и внедрение зависимостей для всех компонентов системы
    /// Поддерживает как Entity Framework, так и Dapper для доступа к данным
    /// </summary>
    public class RabbitNinjectModule : NinjectModule
    {
        private readonly bool _useEntityFramework;

        /// <summary>
        /// Инициализирует модуль с выбором технологии доступа к данным
        /// </summary>
        /// <param name="useEntityFramework">
        /// true - использовать Entity Framework, 
        /// false - использовать Dapper
        /// </param>
        public RabbitNinjectModule(bool useEntityFramework = true)
        {
            _useEntityFramework = useEntityFramework;
        }

        /// <summary>
        /// Загружает конфигурацию привязок зависимостей
        /// Определяет соответствия между интерфейсами и их реализациями
        /// Настраивает жизненный цикл объектов (Singleton, Transient)
        /// </summary>
        public override void Load()
        {
            ConfigureDataAccess();
            ConfigureBusinessServices();
            ConfigureMainLogic();
        }

        /// <summary>
        /// Настраивает технологию доступа к данным (Entity Framework или Dapper)
        /// </summary>
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

        /// <summary>
        /// Настраивает сервисы бизнес-логики
        /// Каждый сервис отвечает за одну конкретную операцию
        /// </summary>
        private void ConfigureBusinessServices()
        {
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
        }

        /// <summary>
        /// Настраивает основной класс бизнес-логики
        /// </summary>
        private void ConfigureMainLogic()
        {
            Bind<ILogic>().To<Logic>().InSingletonScope();
        }
    }
}

