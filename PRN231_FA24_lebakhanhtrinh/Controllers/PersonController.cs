using Microsoft.AspNetCore.Mvc;
using Repository.Request;
using Repository.ViewModel;
using Service.Interface;

namespace PRN231_FA24_lebakhanhtrinh_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        // GET: api/person/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var personDto = await _personService.GetPersonAsync(id);
            if (personDto == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy
            }
            return Ok(personDto); // Trả về 200 OK với dữ liệu
        }

        // POST: api/person
        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] AddPersonRequest addPersonRequest)
        {
            if (addPersonRequest == null)
            {
                return BadRequest("Invalid person data."); // Trả về 400 nếu DTO không hợp lệ
            }

            var result = await _personService.AddPersonAsync(addPersonRequest);

            return CreatedAtAction(nameof(GetPerson), new { id = result.PersonId }, result); // Trả về 201 Created với DTO kết quả
        }
    }
}
