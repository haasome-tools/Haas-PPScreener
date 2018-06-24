using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPScreener
{
    class Utils
    {
        public static void ShowBanner()
        {
            Console.WriteLine("___  ___  ____ ____ ____ ____ ____ _  _ ____ ____ ");
            Console.WriteLine("|__] |__] [__  |    |__/ |___ |___ |\\ | |___ |__/ ");
            Console.WriteLine("|    |    ___] |___ |  \\ |___ |___ | \\| |___ |  \\ ");
            Console.WriteLine("By: R4stl1n");
            Console.WriteLine();
        }

        public static string[] SplitArgumentsSaftley(string arg)
        {
            string[] arguments = arg.Split(' ');

            if (arg.Equals(""))
            {
                arguments = new string[] { };
            }

            return arguments;
        }
    }
}
