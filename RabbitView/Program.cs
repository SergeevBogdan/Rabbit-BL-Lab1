using System;
using System.Windows.Forms;

namespace RabbitView
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Определяем технологию из аргументов
            bool useEF = args.Length == 0 || args[0].ToLower() != "dapper";
            Application.Run(new MainForm(useEF));
        }
    }
}