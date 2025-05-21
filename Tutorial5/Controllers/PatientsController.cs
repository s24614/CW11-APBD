using Microsoft.AspNetCore.Mvc;
using Tutorial5.Services;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IDbService _service;

    public PatientsController(IDbService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var result = await _service.GetPatientDetailsAsync(id);
        if (result == null)
            return NotFound(new { message = $"Pacjent o ID {id} nie istnieje." });

        return Ok(result);
    }
}