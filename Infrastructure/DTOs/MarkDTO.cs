using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class MarkDTO
    {
        public long StudentId { get; set; }
        public string ? StdName { get; set; }
        public string ? SubjectName { get; set; }

        public int SubjectId { get; set; }

        public decimal ? MarkValue { get; set; }
    }
}
