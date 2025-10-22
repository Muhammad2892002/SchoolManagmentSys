using DomainEntities.DBEntities;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
