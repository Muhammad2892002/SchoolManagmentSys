using DomainEntities.DBEntities;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Repositories.IRepositoryFolder;
using Repositories.RepositoryFolder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubjectController : ControllerBase
    {
        public static void Get() { 
           
            
        
        
        
        }
        private readonly IStudentSubjectRepository _studentSubjectRepository;

        [HttpGet("getallmarkstostds")]
        public IActionResult GetAllMarksToStd(long id)
        {

            var reuslt = (from obj in _studentSubjectRepository.Find(x => x.StudentId != 0, x => x.Student, s => s.Subject)
                          where obj.StudentId==id
                          select new MarkDTO { 
                              StudentId = obj.StudentId,
                              StdName=obj.Student.FirstName+" "+obj.Student.LastName,
                              SubjectId=obj.SubjectId,
                              SubjectName=obj.Subject.Name,
                              MarkValue= CheckObj(obj)
                          
                          
                          }).ToList();

            var marksAsJson = JsonConvert.SerializeObject(reuslt, Formatting.None, new JsonSerializerSettings
            {

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(marksAsJson);
           

        }
        public static decimal? CheckObj(StudentSubject std) {
            var result = std.Student.Marks.FirstOrDefault(x => x.SubjectId == std.StudentId);
            if (result == null)
            {
                return 0;


            }
            else {
                return result.Mark1;
            
            
            }





        }

        
    }
}
