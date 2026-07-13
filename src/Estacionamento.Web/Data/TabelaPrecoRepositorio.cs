using Estacionamento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Web.Data;

public class TabelaPrecoRepositorio : ITabelaPrecoRepositorio
{
    private readonly EstacionamentoDbContext _contexto;

    public TabelaPrecoRepositorio(EstacionamentoDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Adicionar(TabelaPreco tabela)
    {
        _contexto.TabelasPreco.Add(tabela);
    }

    public async Task<IEnumerable<TabelaPreco>> ObterTodasAsync()
    {
        return await _contexto.TabelasPreco.ToListAsync();
    }

    public Task SalvarAsync()
    {
        return _contexto.SaveChangesAsync();
    }
}
