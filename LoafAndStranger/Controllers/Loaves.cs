using LoafAndStranger.Models;
using Microsoft.AspNetCore.Mvc;
using LoafAndStranger.DataAccess;

namespace LoafAndStranger.Controllers
{
    [Route("api/Loaves")]
    [ApiController]
    public class LoavesController : ControllerBase
    {
        LoafRepository _repo;
        public LoavesController()
        {
            _repo = new LoafRepository();
        }

        //if its gonna be exposed to the internet, must be PUBLIC
        [HttpGet]
        public IActionResult GetAllLoaves()
        {
            return Ok(_repo.GetAll());
        }

        [HttpPost]
        public IActionResult AddALoaf (Loaf loaf)
        {
            _repo.Add(loaf);
            return Created($"api/Loaves/{loaf.Id}", loaf);
        }
    }
}
