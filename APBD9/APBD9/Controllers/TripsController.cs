using APBD9.Data;
using APBD9.Models;
using APBD9.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APBD9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ScaffoldContext _context;
    private readonly ITripsRepository _tripsRepository;
    private readonly IClientRepository _clientRepository;

    public TripsController(ScaffoldContext context, ITripsRepository tripsRepository,
        IClientRepository clientRepository)
    {
        _context = context;
        _tripsRepository = tripsRepository;
        _clientRepository = clientRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)

    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        var trips = await _tripsRepository.GetTrips(page, pageSize);
        var totalTrips = await _tripsRepository.GetTotalTrips();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var result = new
        {
            pageNum = page,
            pageSize,
            allPages = totalPages,
            trips
        };
        return Ok(result);
    }

    [HttpPost("api/{idTrip}")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, Client client)
    {
        if (await _clientRepository.ClientWithThisPeselExists(client.Pesel))
            return BadRequest("Client with given pesel already exists ");
        if (await _tripsRepository.ClientAlreadyRegistered(client.Pesel, idTrip))
            return BadRequest("Client has already registered for the given trip");
        if (!await _tripsRepository.TripExistsAndIsInTheFuture(idTrip))
            return BadRequest("Trip doesn't exist or has already started");

        await _tripsRepository.RegisterClientTrip(client.Pesel, idTrip);
        return CreatedAtAction(nameof(AssignClientToTrip), new { idTrip = idTrip });
    }
}