using System;
using DAL.DO;
using DAL.DalObject;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 1 to add an item");
            Console.WriteLine("Press 2 to update an item");
            Console.WriteLine("Press 3 to view a specific item");
            Console.WriteLine("Press 4 to view a list of specific item");
            Console.WriteLine("Press 5 to stop");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    {
                        //חלק של תמרי
                        //בפנים יש עוד קייס
                        break;
                    }
                case 2:
                    {

                        //חלק של תמרי
                        //בפנים יש עוד קייס
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                case 4:
                    {
                        break;
                    }
                case 5:
                    {
                        break;
                    }

                default:
                    break;
            }          

        }
    }
}
