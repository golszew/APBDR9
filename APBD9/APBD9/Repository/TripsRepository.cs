using APBD9.Data;
using APBD9.Dtos;
using APBD9.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD9.Repository;

public class TripsRepository : ITripsRepository
{
    private readonly IConfiguration _configuration;
    private readonly ScaffoldContext _context;

    public TripsRepository(IConfiguration configuration, ScaffoldContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<IEnumerable<TripDTO>> GetTrips(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        var trips = await _context.Trips.
            Skip((page -1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO()
        {
            Name = t.Name,
            DateFrom = t.DateFrom,
            MaxPeople = t.MaxPeople,
            // Countries = _context.Countries.Where(c => t.IdCountries.Contains(c.IdCountry)).
            //     Select(c => c.Name)
            //     .ToList(),

            Clients = t.ClientTrips.Select(c => new ClientDTO()
            {
                FirstName = c.IdClientNavigation.FirstName,
                LastName = c.IdClientNavigation.LastName
            }),

        }).ToListAsync();
        
        return trips;
    }

    public async Task<int> GetTotalTrips()
    {
        return await _context.Trips.CountAsync();
    }

    public async Task<bool> ClientAlreadyRegistered(string pesel, int idTrip)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        return trip != null && trip.ClientTrips.Any(ct => ct.IdClientNavigation.Pesel == pesel);
    }

    public async Task<bool> TripExistsAndIsInTheFuture(int idTrip)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        return trip != null && trip.DateFrom >= DateTime.Now;
    }

    public async Task RegisterClientTrip(string pesel, int idTrip)
    {
        var client = _context.Clients.First(c => c.Pesel == pesel);
        var clientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = null
        };
        await _context.ClientTrips.AddAsync(clientTrip);
        await _context.SaveChangesAsync();
    }
}