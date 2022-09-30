using System;

namespace WhileLoop_Test3
{
    class Program
    {
        static void Main(string[] args)
        {
            bool displayMenu = true;
            while (displayMenu)
            {
                displayMenu = MainMenu();       
            }
            
           
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Option 1");
            Console.WriteLine("2) Option 2");
            Console.WriteLine("3) Exit()");

            string result = Console.ReadLine();

            if (result == "1")
            {
                PrintNumbers();
                return true;
            }

            else if (result == "2")
            {
                GuessingGame();
                return true;
            }

            else if (result == "3")
            {
                return false;
            }

            else
            {
                return true;
            }

        }

        private static void PrintNumbers()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Print Numbers! ");

            Console.Write("Type a number: ");
            int userNumber = int.Parse(Console.ReadLine());
            int counter = 1;

            while (counter < userNumber)
            {
                Console.Write(counter + 1);
                Console.Write("-");
                counter++;
            }
            Console.ReadLine();

        }

        private static void GuessingGame()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Guessing Game!");

            Random myRandom = new Random();
            int randomNumber = myRandom.Next(1, 11);

            int guesses = 0;

            bool incorrect = true;

            do
            {
                Console.WriteLine("Guess a number between 1 and 10: ");
                string userInput = Console.ReadLine();
                guesses++;

                if (userInput == randomNumber.ToString())
                {
                    incorrect = false;
                }
                else
                    Console.WriteLine("Wrong!");

            } 
            while (incorrect);

            Console.WriteLine("Correct! It took you {0} guesses.", guesses);

            Console.ReadLine();
        }
    }
}
