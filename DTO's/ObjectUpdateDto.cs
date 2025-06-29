namespace Unity_Backend.DTO_s;

public class ObjectUpdateDto
{
    public string objectId { get; set; }
    public int scaleX { get; set; }
    public int scaleY { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
    public int rotation { get; set; }
}