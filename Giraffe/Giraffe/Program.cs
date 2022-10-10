using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Welcome to Hello World");
            Console.Write("Write your name: ");
            string name = Console.ReadLine();
            Console.Write("Write your age: ");
            string age = Console.ReadLine();
            Console.WriteLine("Hello " + name + " you are " + age + " years old!");
            */

            /*
            int num1 = 32;
            int num2 = 18;
            int ans = num1 + num2;
            Console.WriteLine(ans);
            */

            /*
            int num1 = Convert.ToInt32("23");
            int num2 = Convert.ToInt32("18");
            int ans = num1 + num2;
            Console.WriteLine(ans);
            */

            /*
            Console.WriteLine("Simple Calc");
            Console.Write("Enter the first number: ");
            int userNum1 = int.Parse(Console.ReadLine());
            Console.Write("Enter the second number: ");
            int userNum2 = int.Parse(Console.ReadLine());
            int ans = userNum1 + userNum2;
            Console.WriteLine("Addition of being eneterd numbers is: " + ans);
            */

            /*
            string celebrity, color, capitalTown;

            Console.Write("Enter a celebrity you love: ");
            celebrity = Console.ReadLine();

            Console.Write("Enter a color: ");
            color = Console.ReadLine();

            Console.Write("Enter a capital City: ");
            capitalTown = Console.ReadLine();

            Console.WriteLine("I love " + celebrity);
            Console.WriteLine("Flowers are " + color);
            Console.WriteLine(capitalTown + " is a very famous among us");
            */

            /*
            int[] myArray = { 2, 5, 8, 10, 12 };

            for (int i = 0; i < myArray.Length; i++)
            {
                Console.WriteLine(myArray[i]);
            }
            */

            /*
            string[] friends = new string [6];
            friends[0] = "Tim";  
            friends[1] = "Harry";
            friends[2] = "Mary";
            friends[3] = "Gibson";
            friends[4] = "Gerry";
            friends[5] = "Huston";

            for (int i = 0; i < friends.Length; i++)
            {
                Console.WriteLine(friends[i]);
            }
            */

            /*
            SayHi("Mike", 17);
            SayHi("Jonny", 19);
            SayHi("John", 21);
            SayHi("Tima", 16);
            */

            /*
            int result1 = InPowerOf(8);
            Console.WriteLine(result1);

            int result2 = CubeOf(8);
            Console.WriteLine(result2);
            */

            /*
            bool isMale = false;
            bool isTall = true;

            if (isMale && isTall)
            {
                Console.WriteLine("You are male and tall");
            }
            else
            {
                Console.WriteLine("You are not male and not tall");
            }
            */

            /*
            Console.Write("Enter your first number: ");
            int userNum1 = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter your second number: ");
            int userNum2 = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter your third number: ");
            int userNum3 = Convert.ToInt32(Console.ReadLine());

            int result = (GetMax(userNum1, userNum2, userNum3));            
            Console.WriteLine("Bigger one is: " + result);
            */

            /*
            //Simple Calc
            Console.Write("Enter a number: ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter an operator: ");
            string opr = Console.ReadLine();

            Console.Write("Enter a number: ");
            double num2 = Convert.ToDouble(Console.ReadLine());
            

            if (opr == "+")
            {
                Console.WriteLine(num1 + num2);
            }
            else if (opr == "-")
            {
                Console.WriteLine(num1 - num2);
            }
            else if (opr == "*")
            {
                Console.WriteLine(num1 * num2);
            }
            else if (opr == "/")
            {
                Console.WriteLine(num1 / num2);
            }
            else
            {
                Console.WriteLine("Invalid Operator!");
            }
            */

            /*
            //Switch Statement
            Console.Write("Enter a number between 0 and 6: ");
            int userSelection = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(GetDay(userSelection));
            */

            /*
            //While Loop

            int index = 1;

            while (index <= 5)
            {
                Console.WriteLine(index);
                index++;
            }
            */

            /*
            //Do While Loop
            int index = 6;

            do
            {
                Console.WriteLine(index);
                index++;
            } while (index <= 5);
            */

            //Guessing A word

            /*
            Console.WriteLine("Welcome to Word Guess Game!");

            string secretWord1 = "Home";
            string secretWord2 = "home";
            string guess = "";
            int countLimit = 3;
            int guessCounter = 0;
            bool outOfLimit = false;

            while (guess != secretWord1 && guess != secretWord2 && !outOfLimit)
            {
                if (guessCounter < countLimit)
                {
                    Console.Write("Enter a guess: ");
                    guess = Console.ReadLine();
                    guessCounter++;
                }
                else
                {
                    outOfLimit = true;
                }
            }

            
            if (outOfLimit)
            {
                Console.WriteLine("You have lost!");
            }
            else
            {
                Console.WriteLine("True guess, and you are a winner!");
            }
            */

            /*
            string[] names = { "Jamol", "Azam", "Ahror", "Ahmad", "Alisher" };

            int[] numbers = { 1, 5, 8, 12, 21, 5 };

            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine(names[i]);
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine(numbers[i]);
            }
            */

            /*
            Console.WriteLine(GetPow(4,4));
            */


            /*
            int[,] numberArray = 
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 8, 9, 0 } 
            };
            //int sumOfArray = numberArray[1, 2] * numberArray[0, 2];
            //Console.WriteLine(sumOfArray);

            for (int i = 0; i < numberArray.GetLength(0); i++)
            {
                Console.Write("{ ");
                for (int j = 0; j < numberArray.GetLength(1); j++)
                {
                    
                    Console.Write(numberArray[i, j] + " ");
                    
                }
                
                Console.Write("}");
                Console.Write("\n");
            }
            */


            /*
            int[,] myArray = new int[2, 4];
            
            myArray[0, 0] = 0;
            myArray[0, 1] = 1;
            myArray[0, 2] = 2;
            myArray[0, 3] = 3;
            myArray[1, 0] = 4;
            myArray[1, 1] = 5;
            myArray[1, 2] = 6;
            myArray[1, 3] = 7;
            

            for (int i = 0; i < myArray.GetLength(0); i++)
            {
                Console.Write("\n" + i + ": ");
                for (int j = 0; j <myArray.GetLength(1); j++)
                {
                    Console.Write(myArray[i, j] + " ");
                }
                
            }
            */

            /*
            int[,] numbers = { { 2, 3, 9 }, { 4, 5, 9 } };

            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                Console.Write("Row " + i + ": ");

                for (int j = 0; j < numbers.GetLength(1); j++)
                {
                    Console.Write(numbers[i, j] + " ");

                }
            }
            */

            /*
            //Exception Handling
            try
            {
                Console.Write("Enter fisrt number: ");
                int fNum1 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter second number: ");
                int fNum2 = Convert.ToInt32(Console.ReadLine());

                int result = fNum1 / fNum2;

                Console.WriteLine("fisrt number / by second number equals to: " + result);
            }
            */
            /*
            catch (Exception error)
            {

                Console.WriteLine(error.Message);
            }
            */
            /*
            catch (DivideByZeroException error)
            {
                Console.WriteLine(error.Message);
            }

            catch (FormatException error)
            {
                Console.WriteLine(error.Message);
            }
            */

            /*
            // classes and objects
            Book book1 = new Book();
            book1.author = "JK Rowling";
            book1.title = "Harry Potter";
            book1.pages = 504;
            book1.genres = "Fantastic";

            Book book2 = new Book();
            book2.author = "Ch. Aytmatov";
            book2.title = "Jamila";
            book2.pages = 156;
            book2.genres = "Tragedy";

            Console.WriteLine("{0} {1} {2} {3}", book1.author, book1.title, book1.pages, book1.genres);
            Console.WriteLine("{0} {1} {2} {3}", book2.author, book2.title, book2.pages, book2.genres);
            */

            /*
            School school1 = new School();
            school1.address = "Gimhae, South Korea";
            school1.name = "Inje University";
            school1.phoneNumber = "010-4662-8666";
            school1.id = 1;

            School school2 = new School();
            school2.address = "Busan, South Korea";
            school2.name = "Dongseo University";
            school2.phoneNumber = "233-58-86";
            school2.id = 2;

            Console.WriteLine("{3} {1} {2} {0}", school1.address, school1.name, school1.phoneNumber, school1.id);
            Console.WriteLine("{3} {1} {2} {0}", school2.address, school2.name, school2.phoneNumber, school2.id);

            */

            /*
            Book book1 = new Book("JK Rowling", "Harry Potter", 504, "Fantastic");
            Console.WriteLine("{0} {1} {2} {3}", book1.author, book1.title, book1.pages, book1.genres);
            Book book2 = new Book();
            book2.author = "Ch. Aytmatov";
            book2.title = "Jamila";
            book2.pages = 156;
            book2.genres = "Tragedy";
            Console.WriteLine("{0} {1} {2} {3}", book2.author, book2.title, book2.pages, book2.genres);
            */

            /*
            Students student1 = new Students("Ali", "Art", 3.6);
            Students student2 = new Students("Timur", "Math", 2.8);
            Students student3 = new Students();
            student3.name = "Husan";
            student3.major = "Music";
            student3.gpa = 4.5;
            Console.WriteLine(student1.HasHonors());
            Console.WriteLine(student2.HasHonors());
            Console.WriteLine(student3.HasHonors());
            */

            /*
            Movie movie1 = new Movie("Avengers", "Steve Harley", "Dog");
            Movie movie2 = new Movie("Shrek", "John Murphy", "G");

            Console.WriteLine(movie1.Rating);
            Console.WriteLine(movie2.Rating);
            */

            //UsefulTools myTools = new UsefulTools();

            /*
            UsefulTools.SayHi("Ali");
            UsefulTools.SayBye("Ali");
            */

            Chef chef = new Chef();
            chef.MakeSpecial();

            UzbekChef uzbekChef = new UzbekChef();
            uzbekChef.MakeSpecial();

            Console.ReadLine();            
        }
       

//////////////////////////////////////////////////////////////Methods///////////////////////////////////////////////////////////////////////
        /*
        static void SayHi(string name, int age)
        {
            Console.WriteLine("Hello " + name + " you are " + age + " years old.");
        }
        */

            /*
            static int InPowerOf(int number)
            {
                int inPower = number * number;
                return inPower;
            }

            static int CubeOf(int number)
            {
                int cube = number * number * number;
                return cube;
            }
            */

            /*
            static int GetMax(int num1, int num2, int num3)
            {
                int bigNum;

                if (num1 > num2 && num1 > num3)
                {
                    bigNum = num1;
                }
                else if (num2 > num1 && num2 > num3)
                {
                    bigNum = num2;
                }
                else
                {
                    bigNum = num3;
                }

                return bigNum;
            }
            */

            /*
            static string GetDay(int dayNum)
            {
                string dayName;

                switch (dayNum)
                {
                    case 0:
                        dayName = "Sunday";
                        break;
                    case 1:
                        dayName = "Monday";
                        break;
                    case 2:
                        dayName = "Tuesday";
                        break;
                    case 3:
                        dayName = "Wednesday";
                        break;
                    case 4:
                        dayName = "Thursday";
                        break;
                    case 5:
                        dayName = "Friday";
                        break;
                    case 6:
                        dayName = "Saturday";
                        break;
                    default:
                        dayName = "Invalid day Number!";
                        break;
                }

                return dayName;
            }
            */

            /*
            //4,4
            static int GetPow(int baseNum, int powNum)
            {
                int result = 1;

                for (int i = 0; i < powNum; i++)
                {
                    result = result * baseNum;
                    //1> 1=1*4
                    //2> 4=4*4
                    //3> 16=16*4
                    //4> 64=64*4        <--------------------------
                    //5> 256=256*4
                }

                return result;
            }
            */

        }
    /*
    class School
    {
        public string name;
        public string address;
        public string phoneNumber;
        public int id;
    }
    */
    }

