using APBD9.Dtos;

namespace APBD9.Repository;

public interface ITripsRepository
{
    Task<IEnumerable<TripDTO>> GetTrips(int page, int pageSize);
    Task<int> GetTotalTrips();

    Task<bool> ClientAlreadyRegistered(string pesel, int idTrip);

    Task<bool> TripExistsAndIsInTheFuture(int idTrip);

    Task RegisterClientTrip(string pesel, int idTrip);


}