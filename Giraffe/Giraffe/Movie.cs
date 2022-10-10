using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class Movie
    {
        public string name;
        public string director;
        private string rating;

        public Movie(string aName, string aDirector, string aRating)
        {
            name = aName;
            director = aDirector;  
            Rating = aRating;
        }

        public string Rating
        {
            get { return rating; }
            set
            {
                if (value == "M" || value == "G" || value == "A")
                {
                    rating = value;
                }
                else
                {
                    rating = "NR";
                }
            }
        }
        
    }
}
