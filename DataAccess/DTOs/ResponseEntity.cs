using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ResponseEntity
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
    }
    public enum StatusEnum { Success, Failure }
}
