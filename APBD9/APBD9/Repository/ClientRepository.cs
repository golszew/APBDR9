using APBD9.Data;
using Microsoft.EntityFrameworkCore;

namespace APBD9.Repository;

public class ClientRepository : IClientRepository
{
    private readonly ScaffoldContext _context;
    

    public ClientRepository(ScaffoldContext context)
    {
        _context = context;
    }

    public async Task DeleteClient(int clientId)
    {
        var client = await _context.Clients.FindAsync(clientId);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
            

    }

    public async Task<bool> CanDeleteClient(int clientId)
    {
        return !await _context.ClientTrips.AnyAsync(ct => ct.IdClient == clientId);
    }

    public async Task<bool> ClientWithThisPeselExists(string pesel)
    {
        return await _context.Clients.AnyAsync(c => c.Pesel == pesel);
    }
}