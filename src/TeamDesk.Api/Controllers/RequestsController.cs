using Microsoft.AspNetCore.Mvc;
using TeamDesk.Api.Models;
using TeamDesk.Api.Services;

namespace TeamDesk.Api.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private readonly RequestService _requestService;

    public RequestsController(RequestService requestService)
    {
        _requestService = requestService;
    }

    // GET /api/requests
    [HttpGet]
    public async Task<ActionResult<List<WorkRequest>>> GetAll()
    {
        List<WorkRequest> requests = await _requestService.GetAllAsync();
        return Ok(requests);
    }

    // GET /api/requests/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkRequest>> GetById(int id)
    {
        WorkRequest? request = await _requestService.GetByIdAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        return Ok(request);
    }

    // POST /api/requests
    [HttpPost]
    public async Task<ActionResult<WorkRequest>> Create(WorkRequest input)
    {
        if (string.IsNullOrWhiteSpace(input.Title))
        {
            return BadRequest("Title must not be blank.");
        }

        if (string.IsNullOrWhiteSpace(input.Category))
        {
            return BadRequest("Category must not be blank.");
        }

        if (input.EstimatedCost < 0)
        {
            return BadRequest("EstimatedCost must not be negative.");
        }

        WorkRequest savedRequest = await _requestService.CreateAsync(input);

        // 201 Created, with a Location header pointing to GET /api/requests/{id}.
        return CreatedAtAction(nameof(GetById), new { id = savedRequest.Id }, savedRequest);
    }

    // PUT /api/requests/5/status?status=Approved
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<WorkRequest>> UpdateStatus(int id, [FromQuery] string? status)
    {
        if (status != "Pending" && status != "Approved" && status != "Rejected")
        {
            return BadRequest("Status must be exactly Pending, Approved, or Rejected.");
        }

        WorkRequest? updatedRequest = await _requestService.UpdateStatusAsync(id, status);

        if (updatedRequest == null)
        {
            return NotFound();
        }

        return Ok(updatedRequest);
    }
}
