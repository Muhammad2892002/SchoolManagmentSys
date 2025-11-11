using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class StudentSubjectController : Controller
    {
        public async Task<IActionResult> EditMarkToSub(long stdId, int subId)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7205/api/studentsubject/GetStdAndSub?stdId={stdId}&subId={subId}");
            var stdAndSubs = JsonConvert.DeserializeObject<EditMarkDTO>(await response.Content.ReadAsStringAsync());

            return View(stdAndSubs);
        }


        public async Task<IActionResult> AddMarkToSub(long stdId, int subId)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7205/api/studentsubject/GetStdAndSub?stdId={stdId}&subId={subId}");
            var stdAndSubs = JsonConvert.DeserializeObject<EditMarkDTO>(await response.Content.ReadAsStringAsync());

            return View(stdAndSubs);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubjectsToStd(long stdId,List<int> selectedSubjects) {
            HttpClient client=new HttpClient();
        AddSubjectToStdDTO addSubjectToStdDTO = new AddSubjectToStdDTO();
            addSubjectToStdDTO.StdId = stdId;
            addSubjectToStdDTO.ChosenSub.AddRange(selectedSubjects);
            var subsAsJson = JsonConvert.SerializeObject(addSubjectToStdDTO);
            var response = await client.PostAsync("https://localhost:7205/api/studentsubject/AddStdAndSub", new StringContent(subsAsJson,Encoding.UTF8,"application/json"));

            return RedirectToAction("Index", "Students");
        }




    }
}
