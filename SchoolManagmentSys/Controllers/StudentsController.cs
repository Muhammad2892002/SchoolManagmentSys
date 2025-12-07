using DomainEntities.DBEntities;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Repositories.IRepositoryFolder;


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
        public IActionResult GetAllStudent()
        {

            var students = (from student in _studentRepository.Find(x => x.Id != 0, x => x.GovernorateObj)
                            select new StudentDTO
                            {
                                Id = student.Id,
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                BirthDate = student.BirthDate,
                                CreateDate = student.CreateDate,
                                UpdateDate = student.UpdateDate,
                                Governorate = student.Governorate,
                                GovernorateName = student.GovernorateObj.Name,
                                NationalId = student.NationalId,
                                Gender = student.Gender,
                                GenderName = student.Gender ? "Male" : "Female"





                            }).ToList();


            string jsonsString = JsonConvert.SerializeObject(students, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            });
            return Ok(jsonsString);




        }


        public IActionResult Add(StudentDTO studentDTO)
        {
            List<string> errorMsg = new List<string>();



            try
            {
                var studentExists = _studentRepository.Find(x => x.NationalId == studentDTO.NationalId).Any();
                var isTheSameNameExist = _studentRepository.Find(x => x.FirstName == studentDTO.FirstName && x.LastName == studentDTO.LastName).Any();
                var age = DateTime.Now.Year - studentDTO.BirthDate.Year;
                if (isTheSameNameExist) {

                    errorMsg.Add("The Student Name is already exist");



                  

                }



                if (studentExists == true)
                {

                    errorMsg.Add("Id Exist enter unique Id");
                }
              
                if (age > 19 || age < 5)
                {

                    errorMsg.Add("The student Age must be 5 to 19");

                    //return NotFound("The student must be 5 to 19 ");





                }
                if(errorMsg.Count>0)
                {
                    return NotFound(errorMsg);
                }
                Student student = new Student()
                {
                    FirstName = studentDTO.FirstName,
                    LastName = studentDTO.LastName,
                    BirthDate = studentDTO.BirthDate,
                    Gender = studentDTO.Gender,
                    Governorate = studentDTO.Governorate,
                    CreateDate = DateTime.Now,
                    UpdateDate = null,
                    NationalId = studentDTO.NationalId,



                };
                _studentRepository.Add(student);

                if (ModelState.IsValid)
                {
                    return Ok("Student Added Successfully");
                }
                else
                {
                    return BadRequest("Failed to add student");
                }

            }


            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }





        }

   
        public IActionResult UpdateStd([FromBody]StudentDTO studentDTO)
        {
            List<string> errorMsg = new List<string>();
            var age = DateTime.Now.Year - studentDTO.BirthDate.Year;
            var studentInDb = _studentRepository.Find(x=>x.Id==studentDTO.Id).FirstOrDefault();
            var findStd = _studentRepository.Find(x=>x.NationalId==studentDTO.NationalId &&(x.Id!=studentDTO.Id)).Any();
            var checkName = _studentRepository.Find(x => x.FirstName == studentDTO.FirstName && x.LastName == studentDTO.LastName && x.NationalId != studentInDb.NationalId).Any();
            
            if (studentInDb == null)
            {
                errorMsg.Add("Student not found");
            }
            if (checkName) {
                errorMsg.Add("The Student Name is already exist");

            }
            if (findStd) {
                errorMsg.Add("Enter Unique Id");
            
            }
            if (age < 5 || age > 19) {

                errorMsg.Add("The student age must be 5 to 19.");
            }
            if (errorMsg.Count > 0) { 
              return NotFound(errorMsg);
            }
            else
            {
                studentInDb.FirstName = studentDTO.FirstName;
                studentInDb.LastName = studentDTO.LastName;
                studentInDb.BirthDate = studentDTO.BirthDate;
                studentInDb.Gender = studentDTO.Gender;
                studentInDb.Governorate = studentDTO.Governorate;
                studentInDb.UpdateDate = DateTime.Now;
                studentInDb.NationalId = studentDTO.NationalId;
                studentInDb.CreateDate = studentInDb.CreateDate;
                _studentRepository.Update(studentInDb);
                return Ok("Student Updated Successfully");




            }



        }
        public IActionResult Search(long id) {
            var isStdExist = _studentRepository.Find(x => x.Id == id).FirstOrDefault();
            if (isStdExist == null)
            {

                return NotFound("Student not found");

            }
            else {
            
                return Ok(isStdExist);


            }






        }

        
        public IActionResult Delete(long id)
        {
            var student = _studentRepository.Find(x => x.Id == id).FirstOrDefault();
            if (student == null)
                return NotFound();

            _studentRepository.Delete(student);
            return Ok();
        }

       

    }
}
