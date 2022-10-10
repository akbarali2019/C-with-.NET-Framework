using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class Book
    {
        public string title;
        public string author;
        public int pages;
        public string genres;

        public Book(string aTitle, string aAuthor, int aPages, string aGenres)
        {
            title = aTitle;
            author = aAuthor;
            pages = aPages;
            genres = aGenres;
        }
        public Book()
        {

        }
    }
}
