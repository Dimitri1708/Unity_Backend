using Unity_Backend.DTO_s;

namespace Unity_Backend.Repositories;

public interface IObjectRepository
{
    Task Create(List<ObjectCreateDto> objectCreateDtoList);
    Task<List<ObjectReadDto>> Read(string environmentId);
    Task Update(List<ObjectUpdateDto> objectUpdateDtoList);
    Task Delete(List<string>? objectIdList, string? environmentId);
}