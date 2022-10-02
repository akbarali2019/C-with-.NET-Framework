using System;

namespace SimpleClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            Car myCar = new Car(); //myCar is a new object of a Car class
            myCar.Make = "Hyundai"; //Properties of the instances of the myCar object created using Car class
            myCar.Model = "Sonata";
            myCar.Year = 2009;
            myCar.Color = "White";

            Console.WriteLine(
                "Made Company: {0}\n" +
                "Car Model: {1}\n" +
                "Car Year: {2}\n" +
                "Car Color: {3}\n" +
                "Current Price: {4:C}",
                
                myCar.Make, 
                myCar.Model, 
                myCar.Year, 
                myCar.Color,
                myCar.MarketValue());

            //decimal value = CurrentMarketValue(myCar);
            //Console.WriteLine("{0:C}", value);

            //Console.WriteLine("{0:C}", myCar.MarketValue());

            Console.ReadLine();
        }

        /*
        private static decimal CurrentMarketValue (Car car)
        {
            decimal carValue = 450.0M;
            
            return carValue;
        }
        */
    }

    class Car
    {
        public string Make { get; set; } // instances of the Car class
        public string Model { get; set; } // instances of the Car class
        public int Year { get; set; } // instances of the Car class
        public string Color { get; set; } // instances of the Car class

        public decimal MarketValue()
        {
            decimal carValue;
            if (Year > 2010)
                carValue = 10000; //new cars exoensive
            else
                carValue = 2000; //older cars chaep

            return carValue;
        }
    }
}
