using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class DisplayUnrolledSubDTO
    {
        public long StdId { get; set; }
        public List<SubjectDTO> ?AllUnrolledSubs { get; set; }=new List<SubjectDTO>();
     
    }
}
