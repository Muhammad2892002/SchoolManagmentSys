using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class StudentDTO
    {

        public long Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string? GenderName { get; set; }

        public long NationalId { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int Governorate { get; set; }
        public string ? GovernorateName { get; set; }

        public bool Gender { get; set; }

        public string LastName { get; set; } 
    }
}
