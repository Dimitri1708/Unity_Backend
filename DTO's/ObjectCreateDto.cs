namespace Unity_Backend.DTO_s;

public class ObjectCreateDto
{
    public int scaleX { get; set; }
    public int scaleY { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
    public int rotation { get; set; }
    public string shape { get; set; }
    public string environmentId { get; set; }
}