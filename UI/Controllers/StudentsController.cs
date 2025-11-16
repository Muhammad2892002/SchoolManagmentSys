using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Infrastructure.DTOs;
using System.Text;

namespace UI.Controllers
{
    public class StudentsController : Controller
    {

      private static  bool  IsFailed = false;
        private static string? Errormassage;
        private static StudentDTO _studentDTO = new StudentDTO();
        public async Task<IActionResult> Index()
        {
            List<StudentDTO> AllStudents = new List<StudentDTO>();
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://localhost:7205/api/students/getallstudent");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                     AllStudents=JsonConvert.DeserializeObject<List<StudentDTO>>(await response.Content.ReadAsStringAsync());
                    return View(AllStudents);


                }
                else
                {
                    throw new Exception("Failed to retrieve students.");
                    



                }
                
            }
            catch(Exception ex){ 
                return View("Error",ex.Message);


            }

        }


        public async Task<IActionResult> Add()
        {
           
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://localhost:7205/api/governorate/getallgovernorate");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var governorates = JsonConvert.DeserializeObject<List<GovernorateDTO>>(await response.Content.ReadAsStringAsync());
                    ViewBag.Governorates = governorates;
                    ViewBag.IsFailed = IsFailed;
                    ViewBag.Errormassage = Errormassage;
                    return View(_studentDTO);

                }
                else
                {
                    throw new Exception("Failed to retrieve governorates.");

                    return View();
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);



            }
        }
        [HttpPost("AddStd")]
        public async Task <IActionResult> AddStd(StudentDTO studentDTO)
        {
            try
            {
                var age = DateTime.Now.Year - studentDTO.BirthDate.Year;
                if (age>=19 || age<=5) {

                    _studentDTO = studentDTO;
                    Errormassage = "Please enter valid birth date ";
                    IsFailed = true;
                    return RedirectToAction("Add");



                }
                HttpClient client = new HttpClient();
                var jsonString = JsonConvert.SerializeObject(studentDTO);


                var response = await client.PostAsync("https://localhost:7205/api/students/add", new StringContent(jsonString, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        var massage = response.Content.ReadAsStringAsync();
                        var result = massage.Result;
                        IsFailed = true;
                        Errormassage = result;
                        studentDTO.NationalId = 0;

                        _studentDTO = studentDTO;
                        return RedirectToAction("Add");


                    }
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex) {

                return RedirectToAction("Index");
            }
           



        }

        public async Task <IActionResult> Update(long id) {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7205/api/students/search?id="+id);
            var responseGovernorate = await client.GetAsync("https://localhost:7205/api/governorate/getallgovernorate");
            var governorates = JsonConvert.DeserializeObject<List<GovernorateDTO>>(await responseGovernorate.Content.ReadAsStringAsync());

            var std=JsonConvert.DeserializeObject<StudentDTO>( await response.Content.ReadAsStringAsync());
            if (std == null)
            {


                return RedirectToAction("Index");


            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {


              
                ViewBag.Governorates = governorates;
                return View(std);


            }
            else {

                return RedirectToAction("Index");
            }




            }

        [HttpPost("UpdateStd")]
        public async Task<IActionResult> UpdateStd(StudentDTO studentDTO) {
            HttpClient client = new HttpClient();
            var studentJson=JsonConvert.SerializeObject(studentDTO);
            var response = await client.PutAsync("https://localhost:7205/api/students/UpdateStd", new StringContent(studentJson,Encoding.UTF8,"application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");


            }
            else {
                return RedirectToAction("Index");
            
            
            
            }







        }


        public async Task<IActionResult> Delete(long Id) {
            HttpClient client = new HttpClient();
            // var response = client.DeleteAsync("https://localhost:7205/api/students/Delete/");
            var response = await client.DeleteAsync("https://localhost:7205/api/students/Delete?id="+Id);

            return RedirectToAction("Index");
        
        }
    }
}
