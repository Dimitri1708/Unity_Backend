using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Unity_Backend.DTO_s;
using Unity_Backend.Models;
using Unity_Backend.Repositories;
using Unity_Backend.Utilities;

namespace Unity_Backend.Controllers;
[ApiController]
[Route("Environment")]
public class EnvironmentController(IEnvironmentRepository environmentRepository, IObjectRepository objectRepository, GlobalFunctions globalFunctions) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(EnvironmentCreateDto environmentCreateDto)
    {
        try
        {
            string email = User?.Identity?.Name!;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Email not found in the user context.");
            
            bool disableEnvironmentCreation = await globalFunctions.EnvironmentLimit(email);
            if (disableEnvironmentCreation)
            {
                return StatusCode(403, "You have reached the maximum number of environments.");
            }
            await environmentRepository.Create(environmentCreateDto, email);
            return Created();
        }
        
        catch (SqlException ex)
        {
            return StatusCode(500, $"Database error: {ex.Message}");
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
        
    }

    [HttpGet]
    public async Task<ActionResult<EnvironmentReadDtoListWrapper>> Read()
    {
        try
        {
            var email = User?.Identity?.Name!;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Email not found in the user context.");
            var result = await environmentRepository.Read(email);

            foreach (var environment in result)
            {
                Console.WriteLine($"this is the result {environment.environmentId}");
            }
           
            if (result == null || !result.Any())
            {
                return NotFound(new {message = "No environments where Found"});
            }

            EnvironmentReadDtoListWrapper environmentReadDtoListWrapper = new EnvironmentReadDtoListWrapper(result);
            return Ok(environmentReadDtoListWrapper);
        }

        catch (SqlException ex)
        {
            return StatusCode(500, $"Database error: {ex.Message}");
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string environmentId)
    {
        List<string> fillerList = new List<string>();
        try
        {
            await objectRepository.Delete(fillerList, environmentId);
            await environmentRepository.Delete(environmentId);
            return Ok(new { message = "Environment deleted successfully."});
        }
        
        catch (SqlException ex)
        {
            return StatusCode(500, $"Database error: {ex.Message}");
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Environment with id {environmentId} not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}