using Unity_Backend.DTO_s;

namespace Unity_Backend.Models;

public class ObjectReadDtoListWrapper
{
    public List<ObjectReadDto> objectReadDtoList { get; set; }

    public ObjectReadDtoListWrapper(List<ObjectReadDto> objectList)
    {
        objectReadDtoList = objectList;
    }
}