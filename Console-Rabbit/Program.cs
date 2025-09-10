using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business_logic___rabbit;

namespace Console_Rabbit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logic logic = new Logic();
            //TODO Сделать оформление
            string text = " Выберите опцию: " +
                "\n 1. Создать кролика" +
                "\n 2. Удалить кролика " +
                "\n 3. Прочесть кролика" +
                "\n 4. Изменить кролика" +
                "\n 5. Вычесть средний возраст кролика"+
                "\n 6. Вычесть средний вес кролика"+
                "\n 7. Выход";
            int position = 0;
            while (position != 7)
            {
                Console.WriteLine(text);
                position = Convert.ToInt32(Console.ReadLine());
                switch (position)
                {
                    case 1:
                        Console.WriteLine("text1"); break;
                    case 2:
                        Console.WriteLine("text2"); break;
                    case 3:
                        Console.WriteLine("text3"); break;
                    case 4:
                        Console.WriteLine("text4"); break;
                    case 5:
                        Console.WriteLine("text5"); break;
                    case 6:
                        Console.WriteLine("text6"); break;
                }
            }
        }
    }
}
