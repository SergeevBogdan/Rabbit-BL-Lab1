using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RabbitViewModels;
using BusinessLogicMVP;
using System;

namespace RabbitConsoleMVVM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool useEF = args.Length == 0 || args[0].ToLower() != "dapper";

            Console.WriteLine($"=== НОВАЯ MVVM АРХИТЕКТУРА (CONSOLE) ===");
            Console.WriteLine($"Технология: {(useEF ? "Entity Framework" : "Dapper")}");

            try
            {
                var model = ModelFactory.CreateModel(useEF);
                var viewModel = new MainViewModel(model);
                var consoleView = new ConsoleView();

                consoleView.Initialize(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
