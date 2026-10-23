using System.Net;
using TeamDesk.Web.Models;

namespace TeamDesk.Web.Services;

public class RequestApiService
{
    private readonly HttpClient _httpClient;

    public RequestApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET /api/requests
    public async Task<List<WorkRequest>> GetAllAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/requests");
        await ThrowIfFailedAsync(response);

        List<WorkRequest>? requests = await response.Content.ReadFromJsonAsync<List<WorkRequest>>();
        if (requests == null)
        {
            throw new HttpRequestException("The API returned no request list.");
        }

        return requests;
    }

    // POST /api/requests
    public async Task<WorkRequest> CreateAsync(WorkRequest request)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/requests", request);
        await ThrowIfFailedAsync(response);

        WorkRequest? savedRequest = await response.Content.ReadFromJsonAsync<WorkRequest>();
        if (savedRequest == null)
        {
            throw new HttpRequestException("The API did not return the saved request.");
        }

        return savedRequest;
    }

    // PUT /api/requests/{id}/status?status=Approved
    public async Task<WorkRequest> UpdateStatusAsync(int id, string status)
    {
        string url = $"api/requests/{id}/status?status={Uri.EscapeDataString(status)}";
        HttpResponseMessage response = await _httpClient.PutAsync(url, null);
        await ThrowIfFailedAsync(response);

        WorkRequest? updatedRequest = await response.Content.ReadFromJsonAsync<WorkRequest>();
        if (updatedRequest == null)
        {
            throw new HttpRequestException("The API did not return the updated request.");
        }

        return updatedRequest;
    }

    private async Task ThrowIfFailedAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        int statusCode = (int)response.StatusCode;
        string message = $"The API returned {statusCode} {response.ReasonPhrase}.";

        // A 400 from our API carries a short explanation, such as "Title must not be blank."
        // Other error bodies can be long stack traces, so they are not shown.
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            string explanation = await response.Content.ReadAsStringAsync();
            message = message + " " + explanation;
        }

        throw new HttpRequestException(message, null, response.StatusCode);
    }
}
