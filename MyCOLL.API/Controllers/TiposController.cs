using Microsoft.AspNetCore.Mvc;
using MyCOLL.API.Repositories;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TiposController : ControllerBase {
    private readonly ITipoColecionavelRepository _repository;

    public TiposController(ITipoColecionavelRepository repository) {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<TipoColecionavelDTO>> Get() {
        var items = await _repository.GetAllAsync();
        return items.Select(i => new TipoColecionavelDTO {
            Id = i.Id,
            Nome = i.Nome ?? "Sem Nome",
            Ordem = i.Ordem,
            Imagem = i.Imagem
        });
    }
}
