using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User

    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ForeignKey("Publisher")]
        public int PubId { get; set; }
        public DateTime HireDate { get; set; }
        public Publisher? Publisher { get; set; }
        public Role? Role { get; set; }

    }
}
