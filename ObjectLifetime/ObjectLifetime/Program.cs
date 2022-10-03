using System;

namespace ObjectLifetime
{
    class Program
    {
        static void Main(string[] args)
        {
            Car myCar = new Car();

            myCar.Make = "Hyundai";
            myCar.Model = "Genesis";
            myCar.Year = 2020;
            myCar.Color = "Black";

            Console.WriteLine("{0} {1} {2} {3}", myCar.Make, myCar.Model, myCar.Year, myCar.Color);

            Car mySecondCar;
            mySecondCar = myCar;

            Console.WriteLine("{0} {1} {2} {3}", mySecondCar.Make, mySecondCar.Model, mySecondCar.Year, mySecondCar.Color);

            mySecondCar.Model = "100";
            Console.WriteLine("{0} {1} {2} {3}", myCar.Make, myCar.Model, myCar.Year, myCar.Color);
            Console.WriteLine("{0} {1} {2} {3}", mySecondCar.Make, mySecondCar.Model, mySecondCar.Year, mySecondCar.Color);

            Car.MyMethod();
            
            Car myThirdCar = new Car("Russia", "Jiguli", 1996, "Black");
            Console.WriteLine("{0} {1} {2} {3}", myThirdCar.Make, myThirdCar.Model, myThirdCar.Year, myThirdCar.Color);
            Car myFourthCar = new Car("Germany", "Porshe", 2001, "Yellow");
            Console.WriteLine("{0} {1} {2} {3}", myFourthCar.Make, myFourthCar.Model, myFourthCar.Year, myFourthCar.Color);

            Car myNCar = new Car();
            
            Console.ReadLine();
        }
    }

    class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }


        public Car()
        {
            Make = "Nissan";
            Model = "Toyota";
            Year = 2022;
            Color = "Red";
        }

        
        public Car(string make, string model, int year, string color)
        {
            this.Make = make;
            this.Model = model;
            this.Year = year;
            this.Color = color;
        }
        

        public static void MyMethod()
        {
            Console.WriteLine("Call my static method");
        }

    }
}
