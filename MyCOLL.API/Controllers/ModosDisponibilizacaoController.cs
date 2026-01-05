using Microsoft.AspNetCore.Mvc;
using MyCOLL.API.Repositories;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModosDisponibilizacaoController : ControllerBase {
    private readonly IModoDisponibilizacaoRepository _repository;

    public ModosDisponibilizacaoController(IModoDisponibilizacaoRepository repository) {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<ModoDisponibilizacaoDTO>> Get() {
        var items = await _repository.GetAllAsync();
        return items.Select(i => new ModoDisponibilizacaoDTO {
            Id = i.Id,
            Nome = i.Nome ?? "Sem Nome",
            Descricao = i.Descricao
        });
    }
}
