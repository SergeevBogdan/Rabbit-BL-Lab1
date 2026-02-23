using BusinessLogicMVP;
using RabbitPresenter;
using RabbitSharedMVP;
using RabbitView;
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
            bool useEF = args.Length == 0 || args[0].ToLower() != "dapper";
            var model = ModelFactory.CreateModel(useEF);
            Application.Run(new MainForm(model));
        }
    }
}