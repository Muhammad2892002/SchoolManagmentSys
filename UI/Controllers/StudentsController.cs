using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Infrastructure.DTOs;
using System.Text;

namespace UI.Controllers
{
    public class StudentsController : Controller
    {
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
                    return View();

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
            HttpClient client = new HttpClient();
            var jsonString = JsonConvert.SerializeObject(studentDTO); 
            
            var response = await client.PostAsync("https://localhost:7205/api/students/add", new StringContent(jsonString,Encoding.UTF8,"application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");

            }
            else {
                return RedirectToAction("Index");
            
            }
           



        }
    }
}
