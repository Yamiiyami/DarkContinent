using DarkContinent.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.Controllers;


[Authorize]
public class BaseApiController : ControllerBase
{
    protected ActionResult SuccessResponse(string message, object? data = null, int code = 200)
    {

        return StatusCode(code, new ApiResponse<object>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = code
        });
    }

    protected ActionResult ErrorResponse(string message, int code = 400)
    {
        return StatusCode(code, new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null,
            StatusCode = code
        });
    }

}

// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/[controller]")]
// public class BaseApiController : ControllerBase
// {
//     protected IActionResult SuccessResponse<T>(T data, string message = "Thao tác thành công", int statusCode = 200)
//     {
//         return StatusCode(statusCode, new ApiResponse<T>(statusCode, true, message, data));
//     }

//     protected IActionResult SuccessResponse(string message = "Thao tác thành công", int statusCode = 200)
//     {
//         return StatusCode(statusCode, new ApiResponse<object>(statusCode, true, message, null));
//     }

//     protected IActionResult ErrorResponse(int statusCode, string message)
//     {
//         return StatusCode(statusCode, new ApiResponse<object>(statusCode, false, message, null));
//     }

//     // Ghi đè (Override) các hàm mặc định của ControllerBase để đồng bộ Form
//     [NonAction]
//     public override OkResult Ok() => throw new InvalidOperationException("Hãy dùng SuccessResponse thay thế");
    
//     [NonAction]
//     public override OkObjectResult Ok(object value) => throw new InvalidOperationException("Hãy dùng SuccessResponse thay thế");
// }
