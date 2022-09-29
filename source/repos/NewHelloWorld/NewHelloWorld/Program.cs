using System;

namespace NewHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
              //HelloWorld
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            */

            /*
              //Variables
            int x;
            int y;

            x = 7;
            y = x + 3;

            Console.WriteLine(y);
            Console.ReadLine();
            */

            /*
             //UserInput
            Console.WriteLine("What is your name? ");
            Console.Write("Type your first name: ");
            string myFirstName;
            myFirstName = Console.ReadLine();

            Console.WriteLine("What is your last name? ");
            Console.Write("Type your last name: ");
            string myLastName;
            myLastName = Console.ReadLine();

            Console.WriteLine("Hello, " + myFirstName + " " + myLastName);
            Console.ReadLine();
            */


            /*
            //Decision Statements 

            Console.WriteLine("Ali's Big Giveaway!");
            Console.Write("Choose a door: 1, 2, 3: ");
            string userValue = Console.ReadLine();

            string message = "";

            if (userValue == "1")
                message = "You won a new car!";

            else if (userValue == "2")
                message = "You won a new boat!";

            else if (userValue == "3")
                message = "You won a new motorcycle!";
                
            else
            {
                message = "We did not understand you. ";
                message = message + "You Lost!";
            }

            Console.WriteLine(message);
            Console.ReadLine();

            */

            /*
            Console.WriteLine("Ali's Big Giveaway!");
            Console.Write("Choose a door: 1, 2, 3: ");
            string userValue = Console.ReadLine();

            string message = (userValue == "1") ? "boat" : "nothing";

            //Console.Write("You won a ");
            //Console.Write(message);
            //Console.Write(".");

            Console.WriteLine("You entered: {0}, therefore you won a {1}.", userValue, message);
            Console.ReadLine();
            */

            /*
                        Console.WriteLine("Select 1 to Call to your MoM or Select 2 to call to your DaD");
                        Console.Write("Type the number: ");
                        string UserInput = Console.ReadLine();

                        if (UserInput == "1")
                        {
                            Console.WriteLine("Calling to your MoM...");
                        }
                        else if (UserInput == "2")
                        {
                            Console.WriteLine("Calling to your DaD...");
                        }
                        else
                        {
                            Console.WriteLine("Sorry! Invalid Number!");
                        }
                        Console.ReadLine();
            */

            /*            Console.WriteLine("Select 1 to Call to your MoM otherwise Select any other numbers to call to your DaD");
                        Console.Write("Type the number: ");
                        string UserInput = Console.ReadLine();

                        string message = (UserInput == "1") ? "Calling to MoM..." : "Calling to DaD";

                        Console.WriteLine(message);
                        Console.ReadLine();
            */
            /*           for (int i = 0; i < 10; i++)
                       {
                           //Console.WriteLine(i);
                           //Console.WriteLine("Hello World");

                           if (i == 7)
                           {
                               Console.WriteLine("Here is 7th Iteration!");
                               break;
                           }
                       }

                       for (int myValue = 0; myValue < 15; myValue++)
                       {
                           Console.WriteLine(myValue);
                       }
                       Console.ReadLine();
            */
            //Arrays
            /*
                        int[] numbers = new int [5];

                        numbers[0] = 4;
                        numbers[1] = 8;
                        numbers[2] = 12;
                        numbers[3] = 16;
                        numbers[4] = 20;


                        Console.WriteLine(numbers.Length);
                        Console.ReadLine();
            */

/*            int[] numbers = new int[] { 14, 25, 32, 4, 5 };

            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine(numbers[i]);
            }


            string[] names = new string[] { "Ali", "Aziz", "Tim", "John" };

            for (int j = 0; j < names.Length; j++)
            {
                Console.WriteLine(names[j]);
            }
            Console.ReadLine();
*/
 /*           string[] names = new string[] { "Ali", "Aziz", "Tim", "John" };
            foreach (string name in names)
            {
                Console.WriteLine(name);
            } 
            Console.ReadLine();
 */

            string myFavCitation = "Everything is possible except death, so try to do your best in each situation!";

            char[] keepCitation = myFavCitation.ToCharArray();  //harfma harf qilib arrayga joylaydi, bu degani har bir harf alohida indexga ega boladi.
            Array.Reverse(keepCitation); //Arraydagi harflar ketma ketligini revers qilish uchun ishlatiladigan .NET methodlaridan biri.

            foreach (char revCitation in keepCitation)
            {
                Console.Write(revCitation);
            }
            Console.ReadLine();
        }
    }
}
