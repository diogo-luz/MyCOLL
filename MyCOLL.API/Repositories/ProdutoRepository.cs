using Microsoft.EntityFrameworkCore;
using MyCOLL.Data.Data;
using MyCOLL.Data.Entities;

namespace MyCOLL.API.Repositories;

public class ProdutoRepository : IProdutoRepository {
    private readonly ApplicationDbContext _context;

    public ProdutoRepository(ApplicationDbContext context) {
        _context = context;
    }

    private IQueryable<Produto> GetBaseQuery() {
        return _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.TipoColecionavel)
            .Include(p => p.Pais)
            .Include(p => p.ModoDisponibilizacao)
            .Include(p => p.Fornecedor);
    }

    public async Task<IEnumerable<Produto>> GetAllAsync() {
        return await GetBaseQuery().ToListAsync();
    }
    
    public async Task<IEnumerable<Produto>> GetActiveAsync() {
        return await GetBaseQuery()
            .Where(p => p.Activo && p.Stock > 0)
            .ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(int id) {
        return await GetBaseQuery().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Produto>> GetByFornecedorAsync(string fornecedorId) {
        return await GetBaseQuery()
            .Where(p => p.FornecedorId == fornecedorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId) {
        return await GetBaseQuery()
            .Where(p => p.CategoriaId == categoriaId && p.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> GetByTipoAsync(int tipoId) {
        return await GetBaseQuery()
            .Where(p => p.TipoColecionavelId == tipoId && p.Activo)
            .ToListAsync();
    }

    public async Task<Produto> AddAsync(Produto produto) {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto> UpdateAsync(Produto produto) {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<bool> DeleteAsync(int id) {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return false;
        
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return true;
    }
}
