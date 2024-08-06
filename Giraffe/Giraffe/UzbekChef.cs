using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class UzbekChef : Chef
    {
        public void MakeJuice()
        {
            Console.WriteLine("The Chef makes juice");
        }

        public override void MakeSpecial()
        {
            Console.WriteLine("The Chef makes Palov");
        }

         public override void Serve()
        {
            Console.WriteLine("The Chef does not serve!");
        }
    }
}
