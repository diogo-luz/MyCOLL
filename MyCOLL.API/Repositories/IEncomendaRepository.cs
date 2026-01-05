using MyCOLL.Data.Entities;

namespace MyCOLL.API.Repositories;

public interface IEncomendaRepository {
    Task<IEnumerable<Encomenda>> GetByClienteAsync(string clienteId);
    Task<Encomenda?> GetByIdAsync(int id);
    Task<Encomenda> AddAsync(Encomenda encomenda);
    Task<Encomenda> UpdateEstadoAsync(int id, string novoEstado);
    Task<IEnumerable<ItemEncomenda>> GetVendasByFornecedorAsync(string fornecedorId);
}
