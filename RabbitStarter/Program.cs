using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Console_Rabbit;      
using RabbitConsoleMVP;    

namespace RabbitStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ КРОЛИКАМИ ===");
                Console.WriteLine("ВЫБЕРИТЕ АРХИТЕКТУРУ:");
                Console.WriteLine("1 - MVP Architecture");
                Console.WriteLine("2 - MVVM Architecture");
                Console.WriteLine("3 - Выход");
                Console.Write("Ваш выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LaunchMVP();
                        break;
                    case "2":
                        LaunchMVVM();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        WaitForContinue();
                        break;
                }
            }
        }

        static void LaunchMVP()
        {
            Console.WriteLine("\n=== MVP АРХИТЕКТУРА ===");
            var (interfaceType, useEF) = GetInterfaceAndTechnologyChoice();

            if (interfaceType == "winforms")
            {
                LaunchApplication("RabbitView.exe", useEF ? "ef" : "dapper", "MVP Windows Forms");
            }
            else
            {
                LaunchMVPConsoleDirectly(useEF);
            }
        }

        static void LaunchMVVM()
        {
            Console.WriteLine("\n=== MVVM АРХИТЕКТУРА ===");
            var (interfaceType, useEF) = GetInterfaceAndTechnologyChoice();

            if (interfaceType == "winforms")
            {
                LaunchApplication("WF_Rabbit.exe", useEF ? "ef" : "dapper", "MVVM Windows Forms");
            }
            else
            {
                LaunchMVVMConsoleDirectly(useEF);
            }
        }

        static void LaunchMVPConsoleDirectly(bool useEF)
        {
            try
            {
                Console.WriteLine($"Запуск MVP Console ({GetTechName(useEF)})...");
                RabbitConsoleMVP.Program.Main(new string[] { useEF ? "ef" : "dapper" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                WaitForContinue();
            }
        }

        static void LaunchMVVMConsoleDirectly(bool useEF)
        {
            try
            {
                Console.WriteLine($"Запуск MVVM Console ({GetTechName(useEF)})...");
                Console_Rabbit.Program.Main(new string[] { useEF ? "ef" : "dapper" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                WaitForContinue();
            }
        }

        static void LaunchApplication(string exeName, string args, string appType)
        {
            Console.WriteLine($"Запуск {appType}...");

            try
            {
                string exePath = FindExecutable(exeName);

                if (exePath == null)
                {
                    Console.WriteLine($"Файл {exeName} не найден, соберите проект.");
                    WaitForContinue();
                    return;
                }

                Process.Start(exePath, args);
                Console.WriteLine($" {appType} запущено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static (string interfaceType, bool useEF) GetInterfaceAndTechnologyChoice()
        {
            Console.WriteLine("Выберите интерфейс:");
            Console.WriteLine("1 - Windows Forms (графический)");
            Console.WriteLine("2 - Console (консольный)");
            Console.Write("Ваш выбор: ");

            var interfaceChoice = Console.ReadLine();
            string interfaceType = interfaceChoice == "1" ? "winforms" : "console";

            Console.WriteLine("\nВыберите технологию данных:");
            Console.WriteLine("1 - Entity Framework");
            Console.WriteLine("2 - Dapper");
            Console.Write("Ваш выбор: ");

            var techChoice = Console.ReadLine();
            bool useEF = techChoice == "1";

            return (interfaceType, useEF);
        }

        static string FindExecutable(string exeName)
        {
            if (File.Exists(exeName)) return exeName;

            string[] patterns = {
                Path.Combine("bin", "Debug", exeName),
                Path.Combine("bin", "Release", exeName)
            };

            foreach (string pattern in patterns)
            {
                try
                {
                    string[] files = Directory.GetFiles(".", pattern, SearchOption.AllDirectories);
                    if (files.Length > 0) return files[0];
                }
                catch { }
            }

            return null;
        }

        static string GetTechName(bool useEF) => useEF ? "Entity Framework" : "Dapper";

        static void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}