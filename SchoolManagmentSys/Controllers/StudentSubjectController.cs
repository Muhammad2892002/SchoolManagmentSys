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
    
        private readonly IStudentSubjectRepository _studentSubjectRepository;
        public StudentSubjectController(IStudentSubjectRepository studentRepository) {
            _studentSubjectRepository = studentRepository;
        
        }

        [HttpGet("getallmarkstostds")]
        public IActionResult GetAllMarksToStd(long id)
        {
            var result = _studentSubjectRepository
                .Find(x => x.StudentId == id, x => x.Student, s => s.Subject)
                 .Select(obj => new MarkDTO
                    {
                           StudentId = obj.StudentId,
                            StdName = obj.Student.FirstName + " " + obj.Student.LastName,
                            SubjectId = obj.SubjectId,
                            SubjectName = obj.Subject.Name,
                            MarkValue = obj.Student.Marks
                            .Where(m => m.SubjectId == obj.SubjectId)
                            .Select(m => m.Mark1)
                            .FirstOrDefault()
                             })
                      .ToList();








            var marksAsJson = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings
            {

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(marksAsJson);
           

        }



        [HttpGet("GetAllMarksToSubjects")]
        public IActionResult GetAllMarksToSubject(int id)
        {
            var result = _studentSubjectRepository
                .Find(x => x.SubjectId == id, x => x.Student, s => s.Subject)
                 .Select(obj => new MarkDTO
                 {
                     StudentId = obj.StudentId,
                     StdName = obj.Student.FirstName + " " + obj.Student.LastName,
                     SubjectId = obj.SubjectId,
                     SubjectName = obj.Subject.Name,
                     MarkValue = obj.Student.Marks
                            .Where(m => m.SubjectId == obj.SubjectId)
                            .Select(m => m.Mark1)
                            .FirstOrDefault()??0
                 })
                      .ToList();








            var marksAsJson = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings
            {

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(marksAsJson);


        }


        [HttpGet("GetStdAndSub")]
        public IActionResult GetStdAndSub(long stdId, int subId) {
            var result = _studentSubjectRepository
             .Find(x => x.StudentId == stdId && x.SubjectId == subId, x => x.Student, s => s.Subject)
              .Select(obj => new EditMarkDTO
              {
                  StudentId = obj.StudentId,
                  StdName = obj.Student.FirstName + " " + obj.Student.LastName,
                  SubjectId = obj.SubjectId,
                  SubjectName = obj.Subject.Name,
                  OldMark = obj.Student.Marks
                         .Where(m => m.SubjectId == obj.SubjectId)
                         .Select(m => m.Mark1)
                         .FirstOrDefault()
              }).FirstOrDefault();
            return Ok(result);
                  





        }


        [HttpPost("AddStdAndSub")]
        public IActionResult AddStdAndSub(AddSubjectToStdDTO addSubjectToStdDTO) { 
            StudentSubject studentSubject=new StudentSubject();
            foreach (var item in addSubjectToStdDTO.ChosenSub) {
                studentSubject.StudentId = addSubjectToStdDTO.StdId;
                studentSubject.SubjectId = item;
                _studentSubjectRepository.Add(studentSubject);
               
                
            
            
            }
            return Ok("Success");
            
        
        
        
        
        }






    }
}
