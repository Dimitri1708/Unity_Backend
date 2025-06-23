
using Dapper;
using Microsoft.Data.SqlClient;
using Unity_Backend.DTO_s;
using Unity_Backend.Models;

namespace Unity_Backend.Repositories;

public class EnvironmentRepository(string sqlConnectionString) : IEnvironmentRepository
{
    public async Task Create(EnvironmentCreateDto environmentCreateDto, string email)
    {
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        await sqlConnection.ExecuteAsync($"INSERT INTO [Environment] (EnvironmentName, EnvironmentXScale, EnvironmentYScale, Email) VALUES (@environmentName, @environmentXScale, @environmentYScale, @email)",
            new
            { 
                environmentName = environmentCreateDto.environmentName,
                environmentXScale = environmentCreateDto.environmentXScale,
                environmentYScale = environmentCreateDto.environmentYScale,
                email = email
            });
    }

    public async Task<List<EnvironmentReadDto>> Read(string email)
    {
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        IEnumerable<EnvironmentReadModel> environmentReadModel;
        environmentReadModel = await sqlConnection.QueryAsync<EnvironmentReadModel>($"SELECT * FROM [Environment] WHERE Email = @email", new { email });
        var results = environmentReadModel.Select(environmentReadModel => new EnvironmentReadDto
        {
            environmentId = environmentReadModel.environmentId.ToString(),
            environmentName = environmentReadModel.environmentName,
            environmentXScale = environmentReadModel.environmentXScale,
            environmentYScale = environmentReadModel.environmentYScale,
            email = environmentReadModel.email
        });
        return results.ToList();
    }

    public async Task Delete(string environmentId)
    {
        await using var sqlConnection = new SqlConnection(sqlConnectionString);
        await sqlConnection.ExecuteAsync($"Delete From [Environment] WHERE EnvironmentId = @environmentId",
            new
            {
                environmentId
            });
    }
}