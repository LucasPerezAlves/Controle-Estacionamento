using Estacionamento.Domain;
using Estacionamento.Web.Data;

namespace Estacionamento.Web.Services;

public class RegistradorDeEntrada
{
    private readonly IRegistroEstacionamentoRepositorio _registroRepositorio;

    public RegistradorDeEntrada(IRegistroEstacionamentoRepositorio registroRepositorio)
    {
        _registroRepositorio = registroRepositorio;
    }

    public async Task RegistrarAsync(string placa, DateTime dataHoraEntrada)
    {
        if (string.IsNullOrWhiteSpace(placa))
        {
            throw new ArgumentException("Informe uma placa valida.", nameof(placa));
        }

        if (await _registroRepositorio.BuscarAbertoPorPlacaAsync(placa) is not null)
        {
            throw new InvalidOperationException($"Ja existe um registro aberto para a placa {placa}.");
        }

        var registro = new RegistroEstacionamento(placa, dataHoraEntrada);
        _registroRepositorio.Adicionar(registro);
        await _registroRepositorio.SalvarAsync();
    }
}
