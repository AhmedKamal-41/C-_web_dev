using Microsoft.EntityFrameworkCore;

namespace TeamDesk.Api.Models;

public class WorkRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal EstimatedCost { get; set; }

    public string Status { get; set; } = string.Empty;
}
