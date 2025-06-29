using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Unity_Backend.Models;
using Unity_Backend.Repositories;

namespace Unity_Backend.Controllers;
[ApiController]
[Route("Object")]
public class ObjectController(IObjectRepository objectRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(ObjectCreateDtoListWrapper objectCreateDtoListWrapper)
    {
        try
        {
            await objectRepository.Create(objectCreateDtoListWrapper.objectCreateDtoList);
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
    public async Task<ActionResult<ObjectReadDtoListWrapper>> Read([FromQuery] string environmentId)
    {
        try
        {
            var result = await objectRepository.Read(environmentId);
            if (result == null || !result.Any())
            {
                return NotFound(new {message = "No objects where Found"});
            }
            ObjectReadDtoListWrapper objectReadDtoListWrapper = new ObjectReadDtoListWrapper(result);
            return Ok(objectReadDtoListWrapper);
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
    public async Task<ActionResult> Update(ObjectUpdateDtoListWrapper objectUpdateDtoListWrapper)
    {
        try
        {
            await objectRepository.Update(objectUpdateDtoListWrapper.objectUpdateDtoList);
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
    public async Task<ActionResult> Delete([FromBody] ObjectIdListWrapper objectIdListWrapper)
    {
        string fillerString = "";
        try
        {
            await objectRepository.Delete(objectIdListWrapper.objectIdList, fillerString);
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