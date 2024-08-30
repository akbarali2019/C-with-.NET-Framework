using System;

namespace SimpleClassVers2
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Person object
            Person person = new Person();

            person.Age = 26;
            person.FullName = "Akbarali Otakhanov";
            person.Sex = "Male";
            person.Nationality = "South Korea";

            Console.WriteLine("" +
                "Age: {0}\n" +
                "FullName: {1}\n" +
                "Sex: {2}\n" +
                "{3}",
               
                person.Age, 
                person.FullName, 
                person.Sex, 
                person.Nationality);

            Console.WriteLine("{0}", person.Originilty());
            Console.ReadLine();
        }
    }

    class Person
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }

        public string Originilty()
        {
            string country = "";

            if (Nationality == "South Korea")
                Console.WriteLine("Status: Native");
            else
                Console.WriteLine("Status: Foreigner");

                return country;
        }
    }


}
