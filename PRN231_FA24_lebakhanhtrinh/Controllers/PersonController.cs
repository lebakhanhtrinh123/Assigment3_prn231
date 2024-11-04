using Microsoft.AspNetCore.Mvc;
using Repository.Reponse;
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
        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] AddPersonRequest addPersonRequest)
        {
            if (addPersonRequest == null)
            {
                return BadRequest("Invalid person data."); // Trả về 400 nếu DTO không hợp lệ
            }

            var result = await _personService.AddPersonAsync(addPersonRequest);

            return CreatedAtAction(nameof(GetPersonById), new { id = result.PersonId }, result); // Trả về 201 Created với DTO kết quả
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<GetPersonResponse>>> GetPersons()
        {
            var persons = await _personService.getPersons();
            if (persons == null || persons.Count == 0)
            {
                return NotFound("Không tìm thấy người nào.");
            }
            return Ok(persons); // Trả về 200 OK với danh sách người
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<GetPersonResponse>> GetPersonById(int id)
        {
            var person = await _personService.getPersonById(id);
            return Ok(person);
        }
        [HttpDelete("{personId}")]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            try
            {
                var result = await _personService.DeletePerson(personId);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}

