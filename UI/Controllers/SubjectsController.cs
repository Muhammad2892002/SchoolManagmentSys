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
            return View(jsonAsList);
        }


        public IActionResult Add() { 
            return View();
        
        
        }
        [HttpPost]
        public async Task<IActionResult> Add(SubjectDTO subjectDTO) { 
            HttpClient client = new HttpClient();
            var subjectAsJson= JsonConvert.SerializeObject(subjectDTO);
            var response = await client.PostAsync("https://localhost:7205/api/subjects/Add", new StringContent(subjectAsJson, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");



            }
            else {

                return RedirectToAction("Index");
            
            }
        
        
        
        }
        public async Task<IActionResult> Edit(int id) {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7205/api/subjects/FindSubject?id="+id);
            var jsonAsList = JsonConvert.DeserializeObject<SubjectDTO>(await response.Content.ReadAsStringAsync());
            return View(jsonAsList);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubjectDTO subjectDTO) { 
            HttpClient client = new HttpClient();
            var subjectAsJson= JsonConvert.SerializeObject(subjectDTO);
            var response = await client.PutAsync("https://localhost:7205/api/subjects/Edit", new StringContent(subjectAsJson, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return RedirectToAction("Index");
            }
            else {


                return RedirectToAction("Index");
            }
        
        
        }


        public async Task<IActionResult> Delete(long Id)
        {
            HttpClient client = new HttpClient();
           
            var response = await client.DeleteAsync("https://localhost:7205/api/subjects/Delete?id=" + Id);

            return RedirectToAction("Index");

        }





    }
    }

