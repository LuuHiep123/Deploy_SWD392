using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class test : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("oke");
        }

    }
}
