using DomainEntities.DBEntities;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Repositories.IRepositoryFolder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly IMarkRepository _markRepository;


        public MarkController(IMarkRepository markRepository)
        {
            _markRepository = markRepository;
        }
        [HttpGet("getallmarkstostd")]
        public IActionResult GetAllMarksToStd(long id) {
            
        
          var getAllMarks=(from marks in _markRepository.Find(x=>x.StudentId!=0 ,x=>x.Student,subject=>subject.Subject)
                           
                           where marks.StudentId==id
                           select new MarkDTO {
                               StudentId = marks.StudentId,
                               SubjectId=marks.SubjectId,
                               StdName=marks.Student.FirstName+" "+marks.Student.LastName,
                               SubjectName=marks.Subject.Name,
                               MarkValue=marks.Mark1
                               
                           
                           
                           }
                           ).ToList();
            var marksAsJson=JsonConvert.SerializeObject(getAllMarks,Formatting.None,new JsonSerializerSettings { 
            
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(marksAsJson);
        
        }

        [HttpPost("EditMark")]
        public IActionResult EditMark(EditMarkDTO editMarkDTO)
        {
            Mark mark = new Mark();
            mark=_markRepository.Find(x=>x.StudentId==editMarkDTO.StudentId && x.SubjectId==editMarkDTO.SubjectId).FirstOrDefault();
            mark.Mark1 = editMarkDTO.NewMark;
            _markRepository.Update(mark);

            return Ok("Sucses");






        }
        [HttpPost("AddMark")]
        public IActionResult AddMark(EditMarkDTO markDTO) {
            Mark mark = new Mark();
            mark.StudentId = markDTO.StudentId;
            mark.SubjectId = markDTO.SubjectId;
            
           
            mark.Mark1 = markDTO.OldMark;
            _markRepository.Add(mark);
            return Ok("Success");

        
        
        
        
        }



    }
}
