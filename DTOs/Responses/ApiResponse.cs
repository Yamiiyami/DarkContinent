using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.DTOs.Responses;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

}