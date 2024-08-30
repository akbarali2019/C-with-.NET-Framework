using System;
using System.IO;

namespace HandlingExceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            //intented exception
            RConsole.WriteLine("Hello World!");

            try
            {
                string content = File.ReadAllText(@"C:\ Users\admin\Desktop\trainn.txt");
                Console.WriteLine(content);

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("There was a problem with the filename!");
                           
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("There was a problem with the directoryname!");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem with the filename");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Closing an application...");
            }
            Console.ReadLine();
        }
    }
}
