using MyCOLL.Data.Entities;

namespace MyCOLL.API.Repositories;

public interface IProdutoRepository {
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<Produto?> GetByIdAsync(int id);
    Task<IEnumerable<Produto>> GetByFornecedorAsync(string fornecedorId);
    Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Produto>> GetByTipoAsync(int tipoId);
    Task<IEnumerable<Produto>> GetActiveAsync();
    
    Task<Produto> AddAsync(Produto produto);
    Task<Produto> UpdateAsync(Produto produto);
    Task<bool> DeleteAsync(int id);
}
