using System;
using System.Linq;
using Estacionamento.Domain;

namespace Estacionamento.Web.Data;

public class RegistroEstacionamentoRepositorio
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

    public RegistroEstacionamento? BuscarAbertoPorPlaca(string placa)
    {
        return _contexto.RegistrosEstacionamento.FirstOrDefault(r => r.Placa == placa && r.DataHoraSaida == null);

    }

    public void Salvar()
    {
        _contexto.SaveChanges();
    }
}