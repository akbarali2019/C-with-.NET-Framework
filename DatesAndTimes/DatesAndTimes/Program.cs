using System;

namespace DatesAndTimes
{
    class Program
    {
        static void Main( string [] args)
        {
            DateTime myValue = DateTime.Now;

            //Console.WriteLine(myValue.ToString());
            //Console.WriteLine(myValue.ToShortDateString());
            //Console.WriteLine(myValue.ToShortTimeString());
            //Console.WriteLine(myValue.ToLongDateString());
            //Console.WriteLine(myValue.ToLongTimeString());

            /*
            Console.WriteLine(myValue.ToLongDateString());  //current date
            Console.WriteLine(myValue.ToLongTimeString()); //current time
            Console.WriteLine("");
            Console.WriteLine(myValue.AddDays(3).ToLongDateString()); //after 3 days from current date
            Console.WriteLine(myValue.AddHours(3).ToLongTimeString()); //after 3 days from current time
            Console.WriteLine("");
            Console.WriteLine(myValue.AddDays(-3).ToLongDateString()); //before 3 days from current date
            Console.WriteLine(myValue.AddHours(-3).ToLongTimeString()); //before 3 days from current time
            */

            //Console.WriteLine(myValue.Month + "th month of the year");

            //DateTime myBirthday = new DateTime(1996, 01, 29);
            //Console.WriteLine(myBirthday.ToShortDateString());

            //DateTime myBirthday = DateTime.Parse("01/29/1996");
            //Console.WriteLine(myBirthday.ToShortDateString());

            DateTime myBirthday = DateTime.Parse("01/07/1971"); //datetime object constructor
            TimeSpan myAge = DateTime.Now.Subtract(myBirthday); //timespan object constructor
            Console.WriteLine(myAge.TotalDays);

            Console.ReadLine();

        }
    }
}