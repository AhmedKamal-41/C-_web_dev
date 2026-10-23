using Microsoft.EntityFrameworkCore;
using TeamDesk.Api.Data;
using TeamDesk.Api.Models;

namespace TeamDesk.Api.Services;

public class RequestService
{
    private readonly AppDbContext _db;

    public RequestService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<WorkRequest>> GetAllAsync()
    {
        List<WorkRequest> requests = await _db.WorkRequests
            .OrderBy(request => request.Id)
            .ToListAsync();

        return requests;
    }

    public async Task<WorkRequest?> GetByIdAsync(int id)
    {
        WorkRequest? request = await _db.WorkRequests.FindAsync(id);
        return request;
    }

    public async Task<WorkRequest> CreateAsync(WorkRequest input)
    {
        // Copy only the fields a client may choose. Id comes from the database
        // and every new request starts as Pending, whatever the client sent.
        WorkRequest newRequest = new WorkRequest();
        newRequest.Title = input.Title;
        newRequest.Description = input.Description;
        newRequest.Category = input.Category;
        newRequest.EstimatedCost = input.EstimatedCost;
        newRequest.Status = "Pending";

        _db.WorkRequests.Add(newRequest);
        await _db.SaveChangesAsync();

        return newRequest;
    }

    public async Task<WorkRequest?> UpdateStatusAsync(int id, string status)
    {
        WorkRequest? request = await _db.WorkRequests.FindAsync(id);

        if (request == null)
        {
            return null;
        }

        request.Status = status;
        await _db.SaveChangesAsync();

        return request;
    }
}
