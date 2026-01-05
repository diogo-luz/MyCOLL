using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCOLL.API.Repositories;
using MyCOLL.Data.DTOs;
using MyCOLL.Data.Entities;

namespace MyCOLL.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EncomendasController : ControllerBase {
    private readonly IEncomendaRepository _encomendaRepository;
    private readonly IProdutoRepository _produtoRepository;

    public EncomendasController(IEncomendaRepository encomendaRepository, IProdutoRepository produtoRepository) {
        _encomendaRepository = encomendaRepository;
        _produtoRepository = produtoRepository;
    }

    // GET: api/encomendas/minhas
    [HttpGet("minhas")]
    public async Task<IEnumerable<EncomendaDTO>> GetMinhas() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return new List<EncomendaDTO>();

        var encomendas = await _encomendaRepository.GetByClienteAsync(userId);
        return encomendas.Select(e => new EncomendaDTO {
            Id = e.Id,
            DataEncomenda = e.DataEncomenda,
            Estado = e.Estado,
            Total = e.Total,
            Observacoes = e.Observacoes,
            ClienteNome = e.Cliente?.Nome,
            ClienteEmail = e.Cliente?.Email,
            Itens = (e.Itens ?? new List<ItemEncomenda>()).Select(i => new ItemEncomendaDTO {
                ProdutoId = i.ProdutoId,
                ProdutoNome = i.Produto?.Nome,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        });
    }

    // POST: api/encomendas
    [HttpPost]
    public async Task<ActionResult<Encomenda>> Post([FromBody] CreateEncomendaDTO dto) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var encomenda = new Encomenda {
            ClienteId = userId,
            DataEncomenda = DateTime.UtcNow,
            Estado = "Pendente",
            Observacoes = dto.Observacoes,
            Itens = new List<ItemEncomenda>()
        };

        decimal total = 0;

        foreach (var itemDto in dto.Itens) {
            var produto = await _produtoRepository.GetByIdAsync(itemDto.ProdutoId);
            if (produto == null || !produto.Activo) {
                return BadRequest($"Produto {itemDto.ProdutoId} indisponível.");
            }

            // Opcional: Validar stock
            if (produto.Stock < itemDto.Quantidade) {
                return BadRequest($"Stock insuficiente para o produto {produto.Nome}.");
            }

            var item = new ItemEncomenda {
                ProdutoId = itemDto.ProdutoId,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.PrecoFinal // Preço no momento da compra
            };

            encomenda.Itens.Add(item);
            total += item.Quantidade * item.PrecoUnitario;
        }

        encomenda.Total = total;

        await _encomendaRepository.AddAsync(encomenda);

        return CreatedAtAction(nameof(GetMinhas), null);
    }
}
