using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class AddSubjectToStdDTO
    {
        public long StdId { get; set; }
        public List<int>? ChosenSub { get; set; } = new List<int>();
    }
}
