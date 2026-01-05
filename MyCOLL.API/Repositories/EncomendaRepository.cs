using Microsoft.EntityFrameworkCore;
using MyCOLL.Data.Data;
using MyCOLL.Data.Entities;

namespace MyCOLL.API.Repositories;

public class EncomendaRepository : IEncomendaRepository {
    private readonly ApplicationDbContext _context;

    public EncomendaRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Encomenda>> GetByClienteAsync(string clienteId) {
#pragma warning disable CS8620
        return await _context.Encomendas
            .Include(e => e.Itens)
            .ThenInclude(i => i.Produto)
            .Where(e => e.ClienteId == clienteId)
            .OrderByDescending(e => e.DataEncomenda)
            .ToListAsync();
#pragma warning restore CS8620
    }

    public async Task<Encomenda?> GetByIdAsync(int id) {
#pragma warning disable CS8620
        return await _context.Encomendas
            .Include(e => e.Itens)
            .ThenInclude(i => i.Produto)
            .Include(e => e.Cliente)
            .FirstOrDefaultAsync(e => e.Id == id);
#pragma warning restore CS8620
    }

    public async Task<Encomenda> AddAsync(Encomenda encomenda) {
        _context.Encomendas.Add(encomenda);
        await _context.SaveChangesAsync();
        return encomenda;
    }

    public async Task<Encomenda> UpdateEstadoAsync(int id, string novoEstado) {
        var encomenda = await _context.Encomendas.FindAsync(id);
        if (encomenda == null) throw new KeyNotFoundException("Encomenda não encontrada");

        encomenda.Estado = novoEstado;
        await _context.SaveChangesAsync();
        return encomenda;
    }

    public async Task<IEnumerable<ItemEncomenda>> GetVendasByFornecedorAsync(string fornecedorId) {
        return await _context.ItensEncomenda
            .Include(i => i.Produto)
            .Include(i => i.Encomenda) // Para saber a data da venda
            .Where(i => i.Produto != null && i.Produto.FornecedorId == fornecedorId)
            .OrderByDescending(i => i.Encomenda != null ? i.Encomenda.DataEncomenda : DateTime.MinValue)
            .ToListAsync();
    }
}
