using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace UI.Controllers
{
    public class MarkController : Controller
    {
        public async Task<IActionResult> GetAllMarkToStd(long id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7205/api/mark/getallmarkstostd?id="+id);
            var ListOfMarks = JsonConvert.DeserializeObject<List<MarkDTO>>(await response.Content.ReadAsStringAsync());


            return View(ListOfMarks);
        }



        [HttpPost]
        public async Task<IActionResult> EditMarks(EditMarkDTO markdto)
        {
            HttpClient client = new HttpClient();
            var markAsJson = JsonConvert.SerializeObject(markdto);
            var response = await client.PostAsync("https://localhost:7205/api/mark/EditMark", new StringContent(markAsJson, Encoding.UTF8, "application/json"));
            return RedirectToAction("Index", "Subjects");



        }
        [HttpPost]

        public async Task<IActionResult> AddMarks(EditMarkDTO markDTO) {
            HttpClient client = new HttpClient();
            var markAsJson = JsonConvert.SerializeObject(markDTO);
            var response = await client.PostAsync("https://localhost:7205/api/mark/AddMark", new StringContent(markAsJson, Encoding.UTF8, "application/json"));
            return RedirectToAction("Index", "Subjects");




        }




    }
}
