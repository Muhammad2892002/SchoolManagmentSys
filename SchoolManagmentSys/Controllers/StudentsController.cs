using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Repositories.IRepositoryFolder;
using Infrastructure.DTOs;
using DomainEntities.DBEntities;


namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
      
     


        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
         
        }
        public IActionResult GetAllStudent() {

            var students = (from student in _studentRepository.Find(x=>x.Id!=0,x=>x.GovernorateObj) 
                            select new StudentDTO { 
                                Id=student.Id,
                                FirstName=student.FirstName,
                                LastName=student.LastName,
                                BirthDate=student.BirthDate,
                                CreateDate=student.CreateDate,
                                UpdateDate=student.UpdateDate,
                                Governorate=student.Governorate,
                                GovernorateName=student.GovernorateObj.Name,
                                NationalId=student.NationalId,
                                Gender=student.Gender,
                                GenderName =student.Gender?"Male":"Female"





                            }).ToList();

          
            string jsonsString=JsonConvert.SerializeObject(students,Formatting.None,new JsonSerializerSettings {
              ReferenceLoopHandling= ReferenceLoopHandling.Ignore

            });
            return Ok(jsonsString);
           



        }

       
        public IActionResult Add(StudentDTO studentDTO) {
            Student student = new Student() {
              FirstName=studentDTO.FirstName,
              LastName=studentDTO.LastName,
                BirthDate=studentDTO.BirthDate,
                Gender= studentDTO.Gender,
                Governorate=studentDTO.Governorate,
                CreateDate=DateTime.Now,
                UpdateDate=null,
                NationalId=studentDTO.NationalId,
                
                

            };
            _studentRepository.Add(student);
            
            if(ModelState.IsValid)
            {
                return Ok("Student Added Successfully");
            }
            else
            {
                return BadRequest("Failed to add student");
            }






        }

        
    }
}
