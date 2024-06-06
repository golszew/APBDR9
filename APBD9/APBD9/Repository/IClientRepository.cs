using APBD9.Data;

namespace APBD9.Repository;

public interface IClientRepository
{


    Task DeleteClient(int clientId);
    Task<bool> CanDeleteClient(int clientId);
    Task<bool> ClientWithThisPeselExists(string pesel);
}