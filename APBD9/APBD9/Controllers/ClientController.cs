using APBD9.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APBD9.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;

    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        if (!await _clientRepository.CanDeleteClient(idClient))
            return BadRequest("This client has associated trips");
    
        await _clientRepository.DeleteClient(idClient);
    
        return Ok();
    }
    
}