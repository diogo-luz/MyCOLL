using Microsoft.AspNetCore.Mvc;
using MyCOLL.API.Repositories;
using MyCOLL.Data.DTOs;

namespace MyCOLL.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase {
    private readonly ICategoriaRepository _repository;

    public CategoriasController(ICategoriaRepository repository) {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<CategoriaDTO>> Get() {
        var items = await _repository.GetAllAsync();
        return items.Select(i => new CategoriaDTO {
            Id = i.Id,
            Nome = i.Nome ?? "Sem Nome",
            Ordem = i.Ordem,
            Imagem = i.Imagem
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id) {
        var i = await _repository.GetByIdAsync(id);
        if (i == null) return NotFound();

        return new CategoriaDTO {
            Id = i.Id,
            Nome = i.Nome ?? "Sem Nome",
            Ordem = i.Ordem,
            Imagem = i.Imagem
        };
    }
}
