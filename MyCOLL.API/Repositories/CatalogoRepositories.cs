using Microsoft.EntityFrameworkCore;
using MyCOLL.Data.Data;
using MyCOLL.Data.Entities;

namespace MyCOLL.API.Repositories;

public class CategoriaRepository : ICategoriaRepository {
    private readonly ApplicationDbContext _context;

    public CategoriaRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync() {
        return await _context.Categorias.OrderBy(c => c.Ordem).ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id) {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<Categoria> AddAsync(Categoria categoria) {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria> UpdateAsync(Categoria categoria) {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<bool> DeleteAsync(int id) {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return false;
        
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class TipoColecionavelRepository : ITipoColecionavelRepository {
    private readonly ApplicationDbContext _context;

    public TipoColecionavelRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<TipoColecionavel>> GetAllAsync() {
        return await _context.TiposColecionaveis.OrderBy(t => t.Ordem).ToListAsync();
    }

    public async Task<TipoColecionavel?> GetByIdAsync(int id) {
        return await _context.TiposColecionaveis.FindAsync(id);
    }

    public async Task<TipoColecionavel> AddAsync(TipoColecionavel tipo) {
        _context.TiposColecionaveis.Add(tipo);
        await _context.SaveChangesAsync();
        return tipo;
    }

    public async Task<TipoColecionavel> UpdateAsync(TipoColecionavel tipo) {
        _context.TiposColecionaveis.Update(tipo);
        await _context.SaveChangesAsync();
        return tipo;
    }

    public async Task<bool> DeleteAsync(int id) {
        var tipo = await _context.TiposColecionaveis.FindAsync(id);
        if (tipo == null) return false;
        
        _context.TiposColecionaveis.Remove(tipo);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class PaisRepository : IPaisRepository {
    private readonly ApplicationDbContext _context;

    public PaisRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Pais>> GetAllAsync() {
        return await _context.Paises.OrderBy(p => p.Ordem).ToListAsync();
    }

    public async Task<Pais?> GetByIdAsync(int id) {
        return await _context.Paises.FindAsync(id);
    }

    public async Task<Pais> AddAsync(Pais pais) {
        _context.Paises.Add(pais);
        await _context.SaveChangesAsync();
        return pais;
    }

    public async Task<Pais> UpdateAsync(Pais pais) {
        _context.Paises.Update(pais);
        await _context.SaveChangesAsync();
        return pais;
    }

    public async Task<bool> DeleteAsync(int id) {
        var pais = await _context.Paises.FindAsync(id);
        if (pais == null) return false;
        
        _context.Paises.Remove(pais);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ModoDisponibilizacaoRepository : IModoDisponibilizacaoRepository {
    private readonly ApplicationDbContext _context;

    public ModoDisponibilizacaoRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<ModoDisponibilizacao>> GetAllAsync() {
        return await _context.ModosDisponibilizacao.ToListAsync();
    }

    public async Task<ModoDisponibilizacao?> GetByIdAsync(int id) {
        return await _context.ModosDisponibilizacao.FindAsync(id);
    }

    public async Task<ModoDisponibilizacao> AddAsync(ModoDisponibilizacao modo) {
        _context.ModosDisponibilizacao.Add(modo);
        await _context.SaveChangesAsync();
        return modo;
    }

    public async Task<ModoDisponibilizacao> UpdateAsync(ModoDisponibilizacao modo) {
        _context.ModosDisponibilizacao.Update(modo);
        await _context.SaveChangesAsync();
        return modo;
    }

    public async Task<bool> DeleteAsync(int id) {
        var modo = await _context.ModosDisponibilizacao.FindAsync(id);
        if (modo == null) return false;
        
        _context.ModosDisponibilizacao.Remove(modo);
        await _context.SaveChangesAsync();
        return true;
    }
}
