using System;
using System.Text;

namespace WorkingWithStrings
{
    class Program
    {
        static void Main(string[] args)
        {
            //string myString = "He is \"smart\" boy";

            //string myString = "This year, \nyes this year he said";

            //string myString = "Go to your c:\\ drive";

            //string myString = @"Go to your c:\ drive";

            //string myString = String.Format("{0} = {1}", "First", "Second");

            //string myString = String.Format("{1} = {0}", "First", "Second");

            //string myString = String.Format("{0:C}", 123.45); //Dollar Sign

            //string myString = String.Format("{0:N}", 1234567890); //1,234,567,890.00

            //string myString = String.Format("Percentage: {0:P}", .123); //12.3%

            //string myString = string.Format("Phone Number: {0:(###) ###-####} ", 1234567890); // (123) 456-7890 AmericanStyle
            //string myString = string.Format("Phone Number: {0:(+##) ## #### ####} ", 821046628666); // (1+82) 10 4662 8666 KoreanStyle


            //string myString = " I love her although she is far away  "; 

            //myString = myString.Remove(6, 14); //6 chi indexdan dan boshlab, 14 ta remove qil



            /*string myString = "";

            for(int i = 0; i < 100; i++)
            {
                myString += "--" + i.ToString();
            }

            Console.WriteLine(myString);
            Console.ReadLine();

            */

            //Console.WriteLine("Hello World!");

            StringBuilder myString = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                myString.Append("--");
                myString.Append(i);
            }

            Console.WriteLine(myString);
            Console.ReadLine();
        }
    }
}
