using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class Chef
    {
        public void MakeChicken()
        {
            Console.WriteLine("The Chef makes chicken");
        }

        public void MakeSalad()
        {
            Console.WriteLine("The Chef makes salad");
        }

        public virtual void MakeSpecial()
        {
            Console.WriteLine("The Chef makes cake");
        }

        public virtual void Serve()
        {
            Console.WriteLine("The Chef does not serve!");
        }


    }
}
