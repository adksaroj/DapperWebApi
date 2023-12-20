using DapperWebApi.Models;
using DapperWebApi.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DapperWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private readonly IVillaRepo _repo;

        public VillasController(IVillaRepo repo)
        {
            _repo = repo;
        }

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(typeof(IEnumerable<Villa>), 200)]
        public IActionResult GetAll()
        {
            //throw new CustomException("Custom exception occured.");
            var result = _repo.GetAll().Result;
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Villa), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await _repo.Get(id);
            if (result == null) return NotFound();
            return new OkObjectResult(result);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] Villa villa)
        {
            int status = await _repo.Create(villa);
            if(status == 1)
                return NoContent();
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int id, [FromBody] Villa villa)
        {
            var result = await _repo.Get(id);
            if (result == null) return NotFound();

            int status = await _repo.Update(id, villa);
            if (status == 1)
                return NoContent();
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails() { Detail = "Enxpected error occured." });
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.Get(id);
            if (result == null) return NotFound($"Villas with id {id} not found.");

            int status = await _repo.Delete(id);
            if (status == 1) 
                return NoContent();
            return StatusCode(StatusCodes.Status500InternalServerError,new ProblemDetails() { Detail = "Enxpected error occured."});
        }
    }
}
