namespace Unity_Backend.Models;

public class ObjectReadModel
{
    public Guid objectId { get; set; }
    public int scaleX { get; set; }
    public int scaleY { get; set; }
    
    public int positionX { get; set; }
    public int positionY { get; set; }
    public int rotation { get; set; }
    public string shape { get; set; }
    public Guid environmentId { get; set; }
}