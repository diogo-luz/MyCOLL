using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCOLL.API.Repositories;
using MyCOLL.Shared.DTOs;
using MyCOLL.Data.Entities;

namespace MyCOLL.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase {
    private readonly IProdutoRepository _produtoRepository;

    public ProdutosController(IProdutoRepository produtoRepository) {
        _produtoRepository = produtoRepository;
    }

    // GET: api/produtos
    [HttpGet]
    public async Task<IEnumerable<ProdutoDTO>> Get() {
        var produtos = await _produtoRepository.GetActiveAsync();
        return produtos.Select(MapToDTO);
    }

    // GET: api/produtos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDTO>> Get(int id) {
        var produto = await _produtoRepository.GetByIdAsync(id);
        if (produto == null || (!produto.Activo && !IsDono(produto))) {
            return NotFound();
        }

        Console.WriteLine(
            $"[API] GetProduto({id}) -> CatId: {produto.CategoriaId}, CatNome: {produto.Categoria?.Nome}, TipoId: {produto.TipoColecionavelId}, PaisId: {produto.PaisId}");

        return MapToDTO(produto);
    }

    // GET: api/produtos/meus
    [Authorize(Roles = "Fornecedor")]
    [HttpGet("meus")]
    public async Task<IEnumerable<ProdutoDTO>> GetMeus() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return new List<ProdutoDTO>();

        var produtos = await _produtoRepository.GetByFornecedorAsync(userId);
        return produtos.Select(MapToDTO);
    }

    // GET: api/produtos/categoria/{id}
    [HttpGet("categoria/{id}")]
    public async Task<IEnumerable<ProdutoDTO>> GetPorCategoria(int id) {
        var produtos = await _produtoRepository.GetByCategoriaAsync(id);
        return produtos.Select(MapToDTO);
    }

    // GET: api/produtos/tipo/{id}
    [HttpGet("tipo/{id}")]
    public async Task<IEnumerable<ProdutoDTO>> GetPorTipo(int id) {
        var produtos = await _produtoRepository.GetByTipoAsync(id);
        return produtos.Select(MapToDTO);
    }

    // POST: api/produtos
    [Authorize(Roles = "Fornecedor")]
    [HttpPost]
    public async Task<ActionResult<Produto>> Post([FromBody] ProdutoCreateDTO dto) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var produto = new Produto {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            PrecoBase = dto.PrecoBase,
            Percentagem = 0, // Definido pelo Admin depois
            PrecoFinal = dto.PrecoBase, // Inicialmente igual ao base
            Stock = dto.Stock,
            Estado = dto.Estado ?? "Pendente",
            Ano = dto.Ano,
            ParaVenda = dto.ParaVenda,
            Activo = false, // aprovação pendente
            UrlImagem = dto.UrlImagem,
            Imagem = dto.Imagem,
            CategoriaId = dto.CategoriaId,
            TipoColecionavelId = dto.TipoColecionavelId,
            PaisId = dto.PaisId,
            ModoDisponibilizacaoId = dto.ModoDisponibilizacaoId,
            FornecedorId = userId,
            DataCriacao = DateTime.UtcNow
        };

        var novoProduto = await _produtoRepository.AddAsync(produto);
        return CreatedAtAction(nameof(Get), new { id = novoProduto.Id }, MapToDTO(novoProduto));
    }

    // PUT: api/produtos/{id}
    [Authorize(Roles = "Fornecedor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProdutoCreateDTO dto) {
        var produto = await _produtoRepository.GetByIdAsync(id);
        if (produto == null) return NotFound();

        if (!IsDono(produto)) return Forbid();

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.PrecoBase = dto.PrecoBase;
        produto.Stock = dto.Stock;
        produto.Estado = dto.Estado;
        produto.Ano = dto.Ano;
        produto.ParaVenda = dto.ParaVenda;
        produto.UrlImagem = dto.UrlImagem;
        if (dto.Imagem != null) produto.Imagem = dto.Imagem;
        produto.CategoriaId = dto.CategoriaId;
        produto.TipoColecionavelId = dto.TipoColecionavelId;
        produto.PaisId = dto.PaisId;
        produto.ModoDisponibilizacaoId = dto.ModoDisponibilizacaoId;

        // Se editar, volta a ficar pendente
        produto.Activo = false;

        await _produtoRepository.UpdateAsync(produto);
        return NoContent();
    }

    // DELETE: api/produtos/{id}
    [Authorize(Roles = "Fornecedor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var produto = await _produtoRepository.GetByIdAsync(id);
        if (produto == null) return NotFound();

        if (!IsDono(produto)) return Forbid();

        await _produtoRepository.DeleteAsync(id);
        return NoContent();
    }

    private bool IsDono(Produto p) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return p.FornecedorId == userId;
    }

    private static ProdutoDTO MapToDTO(Produto p) {
        return new ProdutoDTO {
            Id = p.Id,
            Nome = p.Nome ?? "Sem Nome",
            Descricao = p.Descricao,
            PrecoBase = p.PrecoBase,
            PrecoFinal = p.PrecoFinal,
            Percentagem = p.Percentagem,
            Stock = p.Stock,
            Estado = p.Estado,
            Ano = p.Ano,
            ParaVenda = p.ParaVenda,
            Activo = p.Activo,
            UrlImagem = p.UrlImagem,
            Imagem = p.Imagem,

            // Mapear IDs para edição
            CategoriaId = p.CategoriaId,
            TipoColecionavelId = p.TipoColecionavelId,
            PaisId = p.PaisId,
            ModoDisponibilizacaoId = p.ModoDisponibilizacaoId,

            CategoriaNome = p.Categoria?.Nome,
            TipoColecionavelNome = p.TipoColecionavel?.Nome,
            PaisNome = p.Pais?.Nome,
            ModoDisponibilizacaoNome = p.ModoDisponibilizacao?.Nome,
            FornecedorNome = p.Fornecedor?.Nome ?? "Fornecedor"
        };
    }
}
