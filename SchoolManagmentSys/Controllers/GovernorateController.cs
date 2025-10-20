using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.IRepositoryFolder;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IGovernorateRepository _governorateRepository;

        public GovernorateController(IGovernorateRepository governorateRepository)
        {
            _governorateRepository = governorateRepository;
        }

        public IActionResult GetAllGovernorate() {
           var list= _governorateRepository.GetAll();
            var convertjson=JsonConvert.SerializeObject(list,Formatting.None,new JsonSerializerSettings {
                ReferenceLoopHandling= ReferenceLoopHandling.Ignore
            });
            return Ok(convertjson);


        }




    }
}
