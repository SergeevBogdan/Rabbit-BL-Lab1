using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_logic___rabbit;
using Ninject;
using Ninject.Modules;
using RabbitDAL;

namespace Business_logic___rabbit
{
    /// <summary>
    /// Фабрика для создания экземпляров Logic с внедренными зависимостями
    /// Предоставляет единую точку создания объектов бизнес-логики
    /// Поддерживает как Entity Framework, так и Dapper через параметр
    /// </summary>
    public static class LogicFactory
    {
        /// <summary>
        /// Создает экземпляр Logic с выбранной технологией доступа к данным
        /// Использует Ninject для автоматического внедрения зависимостей
        /// </summary>
        /// <param name="useEntityFramework">
        /// true - Entity Framework, 
        /// false - Dapper
        /// </param>
        /// <returns>Готовый экземпляр Logic с внедренными зависимостями</returns>
        public static Logic CreateLogic(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<Logic>();
        }

        /// <summary>
        /// Создает экземпляр ILogic с выбранной технологией доступа к данным
        /// Возвращает интерфейс для лучшей инкапсуляции
        /// </summary>
        /// <param name="useEntityFramework">
        /// true - Entity Framework, 
        /// false - Dapper
        /// </param>
        /// <returns>Готовый экземпляр ILogic с внедренными зависимостями</returns>
        public static ILogic CreateILogic(bool useEntityFramework = true)
        {
            var kernel = new StandardKernel(new RabbitNinjectModule(useEntityFramework));
            return kernel.Get<ILogic>();
        }

        /// <summary>
        /// Создает Logic с конкретным репозиторием (для тестирования)
        /// Позволяет использовать моки или специальные реализации репозитория
        /// </summary>
        /// <param name="repository">Конкретная реализация репозитория</param>
        /// <returns>Готовый экземпляр Logic с переданным репозиторием</returns>
        public static Logic CreateLogicWithRepository(IRepository repository)
        {
            var kernel = new StandardKernel();

            kernel.Bind<IRepository>().ToConstant(repository);
            ConfigureTestBindings(kernel);

            return kernel.Get<Logic>();
        }

        /// <summary>
        /// Настраивает привязки для тестового режима
        /// </summary>
        private static void ConfigureTestBindings(IKernel kernel)
        {
            kernel.Bind<IRabbitAdder>().To<RabbitAdder>();
            kernel.Bind<IRabbitRemover>().To<RabbitRemover>();
            kernel.Bind<IRabbitReader>().To<RabbitReader>();
            kernel.Bind<IRabbitUpdater>().To<RabbitUpdater>();
            kernel.Bind<IRabbitAgeCalculator>().To<RabbitAgeCalculator>();
            kernel.Bind<IRabbitWeightCalculator>().To<RabbitWeightCalculator>();
            kernel.Bind<IRabbitSorter>().To<RabbitSorter>();
            kernel.Bind<IRabbitRandomCreator>().To<RabbitRandomCreator>();
            kernel.Bind<IRabbitDisplayer>().To<RabbitDisplayer>();
            kernel.Bind<IRabbitBreedProvider>().To<RabbitBreedProvider>();
            kernel.Bind<ILogic>().To<Logic>();
            kernel.Bind<Logic>().ToSelf();
        }
    }
}
