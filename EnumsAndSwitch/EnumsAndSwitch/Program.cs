/*Review 2024-06-12*/

using System;
using System.Collections.Generic;

namespace EnumsAndSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            List<ToDo> todos = new List<ToDo>()
            {/*
                new ToDo {Description = "Task1", EstimatedHours = 6, Status = Status.Completed},
                new ToDo {Description = "Task2", EstimatedHours = 2, Status = Status.InProgress},
                new ToDo {Description = "Task3", EstimatedHours = 8, Status = Status.NotStarted},
                new ToDo {Description = "Task4", EstimatedHours = 12, Status = Status.Deleted},
                new ToDo {Description = "Task5", EstimatedHours = 6, Status = Status.InProgress},
                new ToDo {Description = "Task6", EstimatedHours = 2, Status = Status.Completed},
                new ToDo {Description = "Task7", EstimatedHours = 14, Status = Status.OnHold},
                new ToDo {Description = "Task8", EstimatedHours = 8, Status = Status.NotStarted}
                */

                /*
                new ToDo {Description = "Task1", EstimatedHours = 6, Status = Status.Completed},
                new ToDo {Description = "Task2", EstimatedHours = 2, Status = Status.InProgress},
                new ToDo {Description = "Task3", EstimatedHours = 8, Status = Status.NotStarted},
                new ToDo {Description = "Task4", EstimatedHours = 12, Status = Status.Deleted},
                new ToDo {Description = "Task5", EstimatedHours = 6, Status = Status.InProgress},
                new ToDo {Description = "Task6", EstimatedHours = 2, Status = Status.Completed},
                new ToDo {Description = "Task7", EstimatedHours = 14, Status = Status.OnHold},
                new ToDo {Description = "Task8", EstimatedHours = 8, Status = Status.NotStarted}
                */


                /*
                new ToDo {Description = "Task1", EstimatedHours = 6, Status = Status.Completed},
                new ToDo {Description = "Task2", EstimatedHours = 2, Status = Status.InProgress},
                new ToDo {Description = "Task3", EstimatedHours = 8, Status = Status.NotStarted},
                new ToDo {Description = "Task4", EstimatedHours = 12, Status = Status.Deleted},
                new ToDo {Description = "Task5", EstimatedHours = 6, Status = Status.InProgress},
                new ToDo {Description = "Task6", EstimatedHours = 2, Status = Status.Completed},
                new ToDo {Description = "Task7", EstimatedHours = 14, Status = Status.OnHold},
                new ToDo {Description = "Task8", EstimatedHours = 8, Status = Status.NotStarted}
                */


                /*
                new ToDo {Description = "Task1", EstimatedHours = 6, Status = Status.Completed},
                new ToDo {Description = "Task2", EstimatedHours = 2, Status = Status.InProgress},
                new ToDo {Description = "Task3", EstimatedHours = 8, Status = Status.NotStarted},
                new ToDo {Description = "Task4", EstimatedHours = 12, Status = Status.Deleted},
                new ToDo {Description = "Task5", EstimatedHours = 6, Status = Status.InProgress},
                new ToDo {Description = "Task6", EstimatedHours = 2, Status = Status.Completed},
                new ToDo {Description = "Task7", EstimatedHours = 14, Status = Status.OnHold},
                new ToDo {Description = "Task8", EstimatedHours = 8, Status = Status.NotStarted}
                */

                new ToDo {Status = Status.Completed},
                new ToDo {Status = Status.InProgress},
                new ToDo {Status = Status.NotStarted},
                new ToDo {Status = Status.Deleted},
                new ToDo {Status = Status.InProgress},
                new ToDo {Status = Status.Completed},
                new ToDo {Status = Status.OnHold},
                new ToDo {Status = Status.NotStarted}
            };

            //Console.ForegroundColor = ConsoleColor.DarkRed;

            PrintAssessment(todos);
            Console.ReadLine();
        }

        private static void PrintAssessment(List<ToDo> todos)
        {
            foreach (var todo in todos)
            {
                switch (todo.Status)
                {
                    case Status.NotStarted:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case Status.InProgress:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case Status.OnHold:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case Status.Completed:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case Status.Deleted:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        break;
                }
                //Console.WriteLine("{0} {1}",todo.Description, todo.EstimatedHours);
                Console.WriteLine(todo.Status);
            }
        }
    }

    class ToDo
    {
        public string Description { get; set; }
        public int EstimatedHours { get; set; }
        public Status Status { get; set; }
    }

    enum Status
    {
        NotStarted,
        InProgress,
        OnHold,
        Completed,
        Deleted
    }
}
