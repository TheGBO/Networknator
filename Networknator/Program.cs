using Networknator.Utils;
using System;

namespace Networknator
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworknatorLogger.StartLogger(Console.WriteLine, Console.WriteLine, Console.WriteLine);

            Console.ReadLine();
        }
    }
}
