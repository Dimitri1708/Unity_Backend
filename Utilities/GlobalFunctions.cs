using Unity_Backend.Repositories;

namespace Unity_Backend.Utilities;

public class GlobalFunctions(IEnvironmentRepository environmentRepository)
{
    public async Task<bool> EnvironmentLimit(string email)
    {
        var environmentCount = await environmentRepository.Read(email);

        if (environmentCount.Count < 5)
        {
            return false;
        }
        return true;
    }
}