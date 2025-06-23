using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Unity_Backend.DTO_s;
using Unity_Backend.Repositories;

namespace Unity_Backend.Controllers;
[ApiController]
[Route("Object")]
public class ObjectController(IObjectRepository objectRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(List<ObjectCreateDto> objectCreateDtoList)
    {
        try
        {
            string email = User?.Identity?.Name!;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Email not found in the user context.");
            await objectRepository.Create(objectCreateDtoList);
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
    public async Task<ActionResult<List<ObjectReadDto>>> Read([FromQuery] string environmentId)
    {
        try
        {
            var result = await objectRepository.Read(environmentId);
            if (result == null || !result.Any())
            {
                return NotFound(new {message = "No objects where Found"});
            }
            return Ok(result);
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
    
    [HttpPut]
    public async Task<ActionResult> Update(List<ObjectUpdateDto> objectUpdateDtoList)
    {
        try
        {
            await objectRepository.Update(objectUpdateDtoList);
            return Ok(new {message = "Objects successfully updated"});
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
    public async Task<ActionResult> Delete([FromBody] List<string>? objectIdList)
    {
        string fillerString = "";
        try
        {
            await objectRepository.Delete(objectIdList, fillerString);
            return Ok(new { message = "objects deleted successfully."});
        }
        catch(SqlException ex)
        {
            return StatusCode(500, $"Database error: {ex.Message}");
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}