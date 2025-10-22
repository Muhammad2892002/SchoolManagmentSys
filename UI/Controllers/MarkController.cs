using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    }
}
