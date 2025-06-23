using Unity_Backend.DTO_s;

namespace Unity_Backend.Repositories;

public interface IEnvironmentRepository
{
    Task Create(EnvironmentCreateDto environmentCreateDto, string email);
    Task<List<EnvironmentReadDto>> Read(string email);
    Task Delete(string environmentId);
}