using MyCOLL.Data.Entities;

namespace MyCOLL.API.Repositories;

public interface ICategoriaRepository {
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<Categoria> AddAsync(Categoria categoria);
    Task<Categoria> UpdateAsync(Categoria categoria);
    Task<bool> DeleteAsync(int id);
}

public interface ITipoColecionavelRepository {
    Task<IEnumerable<TipoColecionavel>> GetAllAsync();
    Task<TipoColecionavel?> GetByIdAsync(int id);
    Task<TipoColecionavel> AddAsync(TipoColecionavel tipo);
    Task<TipoColecionavel> UpdateAsync(TipoColecionavel tipo);
    Task<bool> DeleteAsync(int id);
}

public interface IPaisRepository {
    Task<IEnumerable<Pais>> GetAllAsync();
    Task<Pais?> GetByIdAsync(int id);
    Task<Pais> AddAsync(Pais pais);
    Task<Pais> UpdateAsync(Pais pais);
    Task<bool> DeleteAsync(int id);
}

public interface IModoDisponibilizacaoRepository {
    Task<IEnumerable<ModoDisponibilizacao>> GetAllAsync();
    Task<ModoDisponibilizacao?> GetByIdAsync(int id);
    Task<ModoDisponibilizacao> AddAsync(ModoDisponibilizacao modo);
    Task<ModoDisponibilizacao> UpdateAsync(ModoDisponibilizacao modo);
    Task<bool> DeleteAsync(int id);
}
