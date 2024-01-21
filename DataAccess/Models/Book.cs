using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public Publisher Publisher { get; set; }
        [ForeignKey("Publisher")]
        public int PubId { get; set; }
        public double? Price { get; set; }
        public string? Advance { get; set; }
        public float? Royalty { get; set; }
        public int? YtdSales { get; set; }
        public string? Notes { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<BookAuthor> bookAuthors { get; set; }
    }
}
