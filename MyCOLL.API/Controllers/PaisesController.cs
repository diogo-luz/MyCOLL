using Microsoft.AspNetCore.Mvc;
using MyCOLL.API.Repositories;
using MyCOLL.Data.DTOs;

namespace MyCOLL.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaisesController : ControllerBase {
    private readonly IPaisRepository _repository;

    public PaisesController(IPaisRepository repository) {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<PaisDTO>> Get() {
        var items = await _repository.GetAllAsync();
        return items.Select(i => new PaisDTO {
            Id = i.Id,
            Nome = i.Nome ?? "Sem Nome",
            CodigoISO = i.CodigoISO,
            Bandeira = i.Bandeira
        });
    }
}
