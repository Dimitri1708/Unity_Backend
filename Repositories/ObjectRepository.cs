using Dapper;
using Microsoft.Data.SqlClient;
using Unity_Backend.DTO_s;
using Unity_Backend.Models;

namespace Unity_Backend.Repositories;

public class ObjectRepository(string sqlConnectionString) : IObjectRepository
{
    public async Task Create(List<ObjectCreateDto> objectCreateDtoList)
    {
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        foreach (var objectCreateDto in objectCreateDtoList)
        {
            await sqlConnection.ExecuteAsync(
                $"INSERT INTO [Object](ScaleX, ScaleY, PositionX, PositionY, Rotation, Shape, EnvironmentId) VALUES(@scaleX, @scaleY, @positionX, @positionY, @rotation, @shape, @environmentId)",
                new
                {
                    scaleX = objectCreateDto.scaleX,
                    scaleY = objectCreateDto.scaleY,
                    positionX = objectCreateDto.positionX,
                    positionY = objectCreateDto.positionY,
                    rotation = objectCreateDto.rotation,
                    shape = objectCreateDto.shape,
                    environmentId = Guid.Parse(objectCreateDto.environmentId)
                });
        }
    }

    public async Task<List<ObjectReadDto>> Read(string environmentId)
    {
        IEnumerable<ObjectReadModel> objectReadModel;
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        objectReadModel = await sqlConnection.QueryAsync<ObjectReadModel>($"SELECT * FROM [Object] WHERE EnvironmentId = @environmentId", new { environmentId });
        var results = objectReadModel.Select(objectReadModel => new ObjectReadDto
        {
            objectId = objectReadModel.objectId.ToString(),
            scaleX = objectReadModel.scaleX,
            scaleY = objectReadModel.scaleY,
            positionX = objectReadModel.positionX,
            positionY = objectReadModel.positionY,
            rotation = objectReadModel.rotation,
            shape = objectReadModel.shape,
            environmentId = environmentId
        });
        return results.ToList();
    }

    public async Task Update(List<ObjectUpdateDto> objectUpdateDtoList)
    {
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        foreach (var objectUpdateDto in objectUpdateDtoList)
        {
              await sqlConnection.ExecuteAsync("UPDATE [Object] SET ScaleX = @scaleX, ScaleY = @scaleY, PositionX = @positionX, PositionY = @positionY, Rotation = @rotation WHERE ObjectID = @objectId",
                        new
                        {
                            scaleX = objectUpdateDto.scaleX,
                            scaleY = objectUpdateDto.scaleY,
                            positionX = objectUpdateDto.positionX,
                            positionY = objectUpdateDto.positionY,
                            rotation = objectUpdateDto.rotation,
                            objectId = objectUpdateDto.objectId
                        });
        }
    }

    public async Task Delete(List<string>? objectIdList, string? environmentId)
    {
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        if (objectIdList == null || !objectIdList.Any())
        {

            await sqlConnection.ExecuteAsync($"DELETE FROM [Object] WHERE EnvironmentId = @environmentId",
                new { environmentId });
            return;
        }

        foreach (var objectId in objectIdList)
        {
            await sqlConnection.ExecuteAsync($"DELETE FROM [Object] WHERE ObjectId = @objectId", new { objectId = Guid.Parse(objectId) });
        }
    }
}