using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class BookAuthor
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string AuthorOrder { get; set; }
        public double RoyalityPercentage { get; set; }
    }
}
