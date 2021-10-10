using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome8754();
            Welcome2775();
            Console.ReadKey();
        }
        static partial void Welcome2775();
        private static void welcome8754()
        {
            Console.WriteLine("Enter your name: ");

            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}
