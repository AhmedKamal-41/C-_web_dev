namespace TeamDesk.Web.Models;

// Matches the JSON returned by the TeamDesk API.
public class WorkRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
    public string Status { get; set; } = string.Empty;
}
