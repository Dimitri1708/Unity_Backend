using Unity_Backend.DTO_s;

namespace Unity_Backend.Models;

public class EnvironmentReadDtoListWrapper
{
    public List<EnvironmentReadDto> environmentReadDtoList { get; set; }

    public EnvironmentReadDtoListWrapper(List<EnvironmentReadDto> environmentsList)
    {
        environmentReadDtoList = environmentsList;
    }
}