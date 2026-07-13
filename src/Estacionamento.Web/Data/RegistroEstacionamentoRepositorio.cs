using Estacionamento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Web.Data;

public class RegistroEstacionamentoRepositorio : IRegistroEstacionamentoRepositorio
{
    private readonly EstacionamentoDbContext _contexto;

    public RegistroEstacionamentoRepositorio(EstacionamentoDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Adicionar(RegistroEstacionamento registro)
    {
        _contexto.RegistrosEstacionamento.Add(registro);
    }

    public Task<RegistroEstacionamento?> BuscarAbertoPorPlacaAsync(string placa)
    {
        return _contexto.RegistrosEstacionamento.FirstOrDefaultAsync(r => r.Placa == placa && r.DataHoraSaida == null);
    }

    public Task SalvarAsync()
    {
        return _contexto.SaveChangesAsync();
    }

    public async Task<IEnumerable<RegistroEstacionamento>> ObterAbertosAsync()
    {
        return await _contexto.RegistrosEstacionamento
            .Where(r => r.DataHoraSaida == null)
            .OrderByDescending(r => r.DataHoraEntrada)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistroEstacionamento>> ObterHistoricoAsync(int pagina, int tamanhoPagina)
    {
        return await _contexto.RegistrosEstacionamento
            .OrderByDescending(r => r.DataHoraEntrada)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();
    }

    public Task<int> ContarTodosAsync()
    {
        return _contexto.RegistrosEstacionamento.CountAsync();
    }
}
