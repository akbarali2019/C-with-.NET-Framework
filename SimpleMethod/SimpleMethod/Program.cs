using System;

namespace SimpleMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            //HelloWorld();
            //Console.ReadLine();

            Console.WriteLine("Reverse Game!");

            Console.Write("What is your Name?: ");
            string myName = Console.ReadLine();

            Console.Write("What is your Surname: ");
            string mySurname = Console.ReadLine();

            Console.Write("Where was you born?: ");
            string myPlace = Console.ReadLine();

            /*Console.WriteLine("Results: ");
            ReverseString(myName);
            ReverseString(mySurname);
            ReverseString(myPlace);
            */

            //Console.WriteLine("Results: ");

            /*string reversedMyName = ReverseString(myName);
            string reversedMySurname = ReverseString(mySurname);
            string reversedMyPlace = ReverseString(myPlace);
            */

            DisplayResult(
                ReverseString(myName), 
                ReverseString(mySurname), 
                ReverseString(myPlace)
                );

            /*Console.Write(String.Format("{0} {1} {2}",
                reversedMyName, 
                reversedMySurname, 
                reversedMyPlace));
            */
            Console.ReadLine();
        }

        /*private static void HelloWorld()
        {
            //Console.WriteLine("Hello World"); 
        }
        */
        
        /*private static void ReverseString(string message)
        {
            char[] charMessage = message.ToCharArray();
            Array.Reverse(charMessage);

            foreach (char revMesage in charMessage)
            {
                Console.Write(revMesage);
            }
            Console.Write(" ");
            
        }
        */

        private static string ReverseString(string message)
        {
            char[] messageArray = message.ToCharArray();
            Array.Reverse(messageArray);

            return String.Concat(messageArray);
        }

        private static void DisplayResult(
            string reversedMyName, 
            string reversedMySurname, 
            string reversedMyPlace
            )
        {
            Console.WriteLine("Results: ");
            Console.Write(
                String.Format("{0} {1} {2}",
                reversedMyName,
                reversedMySurname,
                reversedMyPlace)
                );
        }
    }
}
 