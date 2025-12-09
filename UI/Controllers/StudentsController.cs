using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Infrastructure.DTOs;
using System.Text;
using System.Net;
using Infrastructure.Service;

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

                    if (TempData["msg"] != null)
                    {
                        ViewBag.Msg=TempData["msg"];

                      
                       
                        TempData.Clear();
                    }


                    AllStudents =JsonConvert.DeserializeObject<List<StudentDTO>>(await response.Content.ReadAsStringAsync());
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
                 
                 
                    if (TempData["stdObjJson"] != null)
                    {
                        var errorDetails=JsonConvert.DeserializeObject<List<string>>(TempData["ErrorDetails"].ToString());
                        ViewBag.ErrorDetails =  errorDetails;
                        ViewBag.errorNumber = errorDetails.Count();
                        var student=JsonConvert.DeserializeObject<StudentDTO>(TempData["stdObjJson"].ToString());
                        return View(student);



                    }

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
            try
            {
              
                HttpClient client = new HttpClient();
                var jsonString = JsonConvert.SerializeObject(studentDTO);


                var response = await client.PostAsync("https://localhost:7205/api/students/add", new StringContent(jsonString, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData.Clear();

                    return RedirectToAction("Index");

                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                       
                     
                        TempData["ErrorDetails"] =  response.Content.ReadAsStringAsync().Result;
                        TempData["stdObjJson"]=JsonConvert.SerializeObject(studentDTO);





                        //_studentDTO = studentDTO;
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
            var stddto = new StudentDTO();

            //TempData["ErrorDetails"] = response.Content.ReadAsStringAsync().Result;
            if (TempData["InvalidObj"] != null)
            {
                stddto = JsonConvert.DeserializeObject<StudentDTO>(TempData["InvalidObj"].ToString());
                var errorDetails = JsonConvert.DeserializeObject<List<string>>(TempData["ErrorDetails"].ToString());
                TempData["StatusCode"] = System.Net.HttpStatusCode.OK.ToString();
                ViewBag.errorNumbers = errorDetails.Count();
                ViewBag.ErrorDetails = errorDetails;


            }
            else {
                var response = await client.GetAsync("https://localhost:7205/api/students/search?id="+id);
                TempData["StatusCode"] = response.StatusCode.ToString();
                
                stddto =JsonConvert.DeserializeObject<StudentDTO>( await response.Content.ReadAsStringAsync());
            
            }
              
            
            var responseGovernorate = await client.GetAsync("https://localhost:7205/api/governorate/getallgovernorate");
            var governorates = JsonConvert.DeserializeObject<List<GovernorateDTO>>(await responseGovernorate.Content.ReadAsStringAsync());

            
            if (stddto == null)
            {


                return RedirectToAction("Index");


            }
            else if (TempData["StatusCode"] == System.Net.HttpStatusCode.OK.ToString())
            {
                //if (status == System.Net.HttpStatusCode.NotFound)
                //{
               
                    
                //}

                //else { 
                
                  
                //}


                
                ViewBag.Governorates = governorates;
                return View(stddto);


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
                TempData.Clear();

                return RedirectToAction("Index");


            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["InvalidObj"]=JsonConvert.SerializeObject(studentDTO);
                TempData["ErrorDetails"] = response.Content.ReadAsStringAsync().Result;




                return RedirectToAction("Update");





            }
            else { 
               
                return RedirectToAction("Index");

            }







        }


        public async Task<IActionResult> Delete(long Id) {
            HttpClient client = new HttpClient();
            // var response = client.DeleteAsync("https://localhost:7205/api/students/Delete/");
            var response = await client.DeleteAsync("https://localhost:7205/api/students/Delete?id="+Id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
               
                TempData["msg"]= await response.Content.ReadAsStringAsync();

            }
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict) {
              
                TempData["msg"] = await response.Content.ReadAsStringAsync();


            }

            return RedirectToAction("Index");
        
        }
    }
}
