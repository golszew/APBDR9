using APBD9.Models;

namespace APBD9.Dtos;

public class TripDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    // public IEnumerable<string> Countries { get; set; } = new List<string>();
    public IEnumerable<ClientDTO> Clients { get; set; } = new List<ClientDTO>();
    
    
}