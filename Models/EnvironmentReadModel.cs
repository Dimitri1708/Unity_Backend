namespace Unity_Backend.Models;

public class EnvironmentReadModel
{
    public Guid environmentId { get; set; }
    public string environmentName { get; set; }
    public int environmentXScale { get; set; }
    public int environmentYScale { get; set; }
    public string email { get; set; }
}