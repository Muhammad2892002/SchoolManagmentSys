using DomainEntities.DBEntities;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.IRepositoryFolder;
using Repositories.RepositoryFolder;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectsController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public IActionResult GetAllSubjects() {

            var subjects = (from obj in _subjectRepository.GetAll()
                            select new SubjectDTO { 
                                Id= obj.Id,
                                Name=obj.Name,
                                
                            
                            }).ToList();
            var subjectAsJson=JsonConvert.SerializeObject(subjects,Formatting.None,new JsonSerializerSettings {
             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(subjectAsJson);
        }

        public IActionResult Add(SubjectDTO subjectDTO) { 
            Subject subject = new Subject() { 
             Name= subjectDTO.Name};
            _subjectRepository.Add(subject);
            return Ok(subject);
             
            
           
            
        
        
        
        
        
        }

        public IActionResult FindSubject(int id) {
            var isSubjectExist = _subjectRepository.Find(x => x.Id == id).FirstOrDefault();
            return Ok(isSubjectExist);



        }


        public IActionResult Edit(SubjectDTO subjectDTO) {
            Subject subject = new Subject()
            {
                Id= subjectDTO.Id,
                Name= subjectDTO.Name,

            };
            _subjectRepository.Update(subject);
            return Ok("Adding Successfully");
        
        
        }

        public IActionResult Delete(int id) {
            var subjects = _subjectRepository.Find(x => x.Id == id).FirstOrDefault();
            _subjectRepository.Delete(subjects);
            return Ok("Deleted Success");
        
        
        }


        public IActionResult GetAllUnrolledSubjects(long id) {
            var result = (from obj in _subjectRepository.Find(x => x.Id != 0, substd => substd.StudentSubjects)
                          where !obj.StudentSubjects.Any(ss => ss.StudentId == id)
                          select new SubjectDTO
                          {
                              Id = obj.Id,
                              Name = obj.Name

                          }).ToList();
            DisplayUnrolledSubDTO displayUnrolledSubDTO = new DisplayUnrolledSubDTO();
            displayUnrolledSubDTO.AllUnrolledSubs.AddRange(result);
            displayUnrolledSubDTO.StdId = id;
            return Ok(displayUnrolledSubDTO);
        
        
        
        }




    }
}
