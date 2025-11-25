using System;
using Business_logic___rabbit;
using RabbitPresenter;
using RabbitShared;

namespace RabbitConsoleMVP
{
    public class Program  // ← ДОБАВЬТЕ PUBLIC!
    {
        public static void Main(string[] args)  // ← ДОБАВЬТЕ PUBLIC!
        {
            // Определяем технологию из аргументов
            bool useEF = args.Length == 0 || args[0].ToLower() != "dapper";

            Console.WriteLine($"=== MVP CONSOLE ({GetTechName(useEF)}) ===");

            try
            {
                var logic = LogicFactory.CreateLogic(useEF);
                var view = new ConsoleView();
                var presenter = new Presenter(view, logic);

                // Инициализируем View с породами
                var breeds = presenter.GetBreeds();
                view.Initialize(breeds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска: {ex.Message}");
                Console.ReadKey();
            }
        }

        static string GetTechName(bool useEF) => useEF ? "Entity Framework" : "Dapper";
    }
}