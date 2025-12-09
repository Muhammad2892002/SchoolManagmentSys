using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class SubjectsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7205/api/subjects/getallsubjects");
            var jsonAsList = JsonConvert.DeserializeObject<List<SubjectDTO>>(await response.Content.ReadAsStringAsync());
          
            if (TempData["SuccessMessage"]!=null) { 
                ViewBag.Msg = TempData["SuccessMessage"];


                TempData.Clear();


            }
        

            return View(jsonAsList);
        }


        public IActionResult Add() {
           
            if (TempData["msg"] != null) {
                ViewBag.Msg = TempData["msg"];
                TempData.Clear();

            }
            return View();
        
        
        }
        [HttpPost]
        public async Task<IActionResult> Add(SubjectDTO subjectDTO) { 
            HttpClient client = new HttpClient();
            var subjectAsJson= JsonConvert.SerializeObject(subjectDTO);
            var response = await client.PostAsync("https://localhost:7205/api/subjects/Add", new StringContent(subjectAsJson, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["SuccessMessage"] = "Subject Added Successfully";
                return RedirectToAction("Index");



            }
            else {
                TempData["msg"] = $"The Subject you Typed {subjectDTO.Name} is already exist please enter another subjectname";

                return RedirectToAction("Add");
            
            }
        
        
        
        }
        public async Task<IActionResult> Edit(int id) {
            HttpClient client = new HttpClient();
            SubjectDTO stdDTO = new SubjectDTO();
            //TempData["object"] = JsonConvert.SerializeObject(subjectDTO);
            //TempData["SuccessMsg"] = response.Content.ReadAsStringAsync().Result;
            if (TempData["object"] != null && TempData["Msg"] != null)
            {
                stdDTO=JsonConvert.DeserializeObject<SubjectDTO>(TempData["object"].ToString());
                ViewBag.Msg = TempData["Msg"];
                TempData.Clear();


            }
            else
            {

                var response = await client.GetAsync("https://localhost:7205/api/subjects/FindSubject?id=" + id);
                stdDTO = JsonConvert.DeserializeObject<SubjectDTO>(await response.Content.ReadAsStringAsync());
                ViewBag.Msg = TempData["Msg"];
                TempData.Clear();
            }
            

            return View(stdDTO);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubjectDTO subjectDTO) { 
            HttpClient client = new HttpClient();
            var subjectAsJson= JsonConvert.SerializeObject(subjectDTO);
            var response = await client.PutAsync("https://localhost:7205/api/subjects/Edit", new StringContent(subjectAsJson, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Msg"]=response.Content.ReadAsStringAsync().Result;
                
                return RedirectToAction("Index");
            }
            else {
                TempData["object"]=JsonConvert.SerializeObject(subjectDTO);
                TempData["Msg"] = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("Edit");
            }
        
        
        }


        public async Task<IActionResult> Delete(long Id)
        {
            HttpClient client = new HttpClient();
           
            var response = await client.DeleteAsync("https://localhost:7205/api/subjects/Delete?id=" + Id);

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> GetAllEnrolledStudents(int id) {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7205/api/studentsubject/GetAllMarksToSubjects?id=" + id);
            var stdAssubjects = JsonConvert.DeserializeObject<List<MarkDTO>>(await response.Content.ReadAsStringAsync());
            ViewBag.SubjectName=stdAssubjects.FirstOrDefault()?.SubjectName;
            return View(stdAssubjects);
        
        
        
        }


        public async Task<IActionResult> GetAllUnrolledSubjects(long id) {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync("https://localhost:7205/api/subjects/GetAllUnrolledSubjects?id=" + id);
            var SubjectsAsList = JsonConvert.DeserializeObject<DisplayUnrolledSubDTO>(await result.Content.ReadAsStringAsync());
            return View(SubjectsAsList);
        
        
        }


        



    }
    }

