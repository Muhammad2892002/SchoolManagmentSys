using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Infrastructure.DTOs;
using System.Text;
using System.Net;

namespace UI.Controllers
{
    public class StudentsController : Controller
    {

      private static  int  errorNumber = 0;
        public static HttpStatusCode status=System.Net.HttpStatusCode.OK;
      
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
                    ViewBag.errorNumber = errorNumber;
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
              
                HttpClient client = new HttpClient();
                var jsonString = JsonConvert.SerializeObject(studentDTO);


                var response = await client.PostAsync("https://localhost:7205/api/students/add", new StringContent(jsonString, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    errorNumber = 0;
                    _studentDTO = null;
                    Errormassage = null;
                    
                    return RedirectToAction("Index");

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                       
                        var massage = response.Content.ReadAsStringAsync();
                        var result = massage.Result;
                        errorNumber = ModelState.Count;
                        Errormassage = result;
                        

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
                if (status == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.IsFaild = true;
                    ViewBag.msg = Errormassage;
                    
                }

                else { 
                
                    ViewBag.IsFaild = false;
                ViewBag.msg = Errormassage;
                }


                
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
                ViewBag.isFailed = false;
                status = response.StatusCode;
                return RedirectToAction("Index");


            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
              
                Errormassage=response.Content.ReadAsStringAsync().Result;
                status = response.StatusCode;
                return RedirectToAction("Update", new { id = studentDTO.Id });





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
