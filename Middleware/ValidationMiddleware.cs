using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XTracker.DTOs.UserDTOs;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/User/update") && context.Request.Method == "PATCH")
        {
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var updateUserDTO = JsonSerializer.Deserialize<UpdateUserDTO>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(updateUserDTO, null, null);
            if (!Validator.TryValidateObject(updateUserDTO, validationContext, validationResults, true))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = validationResults.Select(e => e.ErrorMessage).ToArray();
                var response = new { status = "Error", message = string.Join("; ", errors) };

                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            if (!string.IsNullOrEmpty(updateUserDTO.NewPassword) &&
                updateUserDTO.NewPassword != updateUserDTO.ConfirmNewPassword)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var response = new { status = "Error", message = "As senhas não coincidem." };
                await context.Response.WriteAsJsonAsync(response);
                return;
            }
        }

        await _next(context);
    }
}
