using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class Students
    {
        public string name;
        public string major;
        public double gpa;

        public Students(string aName, string aMajor, double aGpa)
        {
            this.name = aName;
            major = aMajor;
            gpa = aGpa;
        }
        public Students()
        {

        }
        public bool HasHonors()
        {
            if (gpa >= 3.5)
            {
                return true;
            }
            return false;
        }
    }


}
