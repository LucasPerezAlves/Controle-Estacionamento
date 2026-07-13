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

    public async Task<IEnumerable<RegistroEstacionamento>> ObterTodosAsync()
    {
        return await _contexto.RegistrosEstacionamento
            .OrderByDescending(r => r.DataHoraEntrada)
            .ToListAsync();
    }
}
