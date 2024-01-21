using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Books
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public int PubId { get; set; }
        public double Price { get; set; }
        public float Royalty { get; set; }
        public int YtdSales { get; set; }
        public string Notes { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Advance { get; set; }
        public int AuthorId { get; set; }
    }
}
