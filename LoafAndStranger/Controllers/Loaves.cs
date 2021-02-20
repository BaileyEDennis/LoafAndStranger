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
        //GET to /api/loaves
        [HttpGet]
        public IActionResult GetAllLoaves()
        {
            return Ok(_repo.GetAll());
        }
        //POST to /api/loaves
        [HttpPost]
        public IActionResult AddALoaf (Loaf loaf)
        {
            _repo.Add(loaf);
            return Created($"api/Loaves/{loaf.Id}", loaf);
        }

        //GET to /api/loaves/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var loaf = _repo.Get(id);
            if (loaf == null)
            {
                return NotFound("This loaf id does not exist");
            }
            return Ok(loaf);
        }

        //idempotency -> idempotent
        //PUT to /api/loaves/{id}/slice
        [HttpPut("{id}/slice")]
        public IActionResult SliceLoaf(int id)
        {
            var loaf = _repo.Get(id);
            if (loaf.Sliced)
            {
                return NoContent(); //204 status. Hey we already sliced it!
            }

            loaf.Sliced = true;

            return NoContent();
        }

        //DELETE /api/loaves/{loafId}
        [HttpDelete("{loafId}")]
        public IActionResult PurchaseLoaf(int loafId)
        {
            _repo.Remove(loafId);

            return Ok();
        }
    }
}
