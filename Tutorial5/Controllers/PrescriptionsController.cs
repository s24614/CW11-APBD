namespace Tutorial5.Controllers;

using Microsoft.AspNetCore.Mvc;
using Tutorial5.DTOs;
using Tutorial5.Services;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _service;

    public PrescriptionsController(IDbService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto dto)
    {
        try
        {
            var id = await _service.AddPrescriptionAsync(dto);
            return CreatedAtAction(nameof(AddPrescription), new { id }, new { IdPrescription = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
