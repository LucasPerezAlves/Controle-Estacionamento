using Estacionamento.Domain;
using Estacionamento.Web.Data;

namespace Estacionamento.Web.Services;

public class RegistradorDeSaida
{
    private readonly IRegistroEstacionamentoRepositorio _registroRepositorio;
    private readonly ITabelaPrecoRepositorio _tabelaPrecoRepositorio;
    private readonly SeletorDeTabelaPreco _seletorDeTabelaPreco;
    private readonly CalculadoraTarifa _calculadoraTarifa;

    public RegistradorDeSaida(
        IRegistroEstacionamentoRepositorio registroRepositorio,
        ITabelaPrecoRepositorio tabelaPrecoRepositorio,
        SeletorDeTabelaPreco seletorDeTabelaPreco,
        CalculadoraTarifa calculadoraTarifa)
    {
        _registroRepositorio = registroRepositorio;
        _tabelaPrecoRepositorio = tabelaPrecoRepositorio;
        _seletorDeTabelaPreco = seletorDeTabelaPreco;
        _calculadoraTarifa = calculadoraTarifa;
    }

    public async Task RegistrarAsync(string placa, DateTime dataHoraSaida)
    {
        var registro = await _registroRepositorio.BuscarAbertoPorPlacaAsync(placa);

        if (registro is null)
        {
            throw new InvalidOperationException($"Nao existe um registro aberto para a placa {placa}.");
        }

        var tabelas = await _tabelaPrecoRepositorio.ObterTodasAsync();
        var tabelaVigente = _seletorDeTabelaPreco.Selecionar(tabelas, registro.DataHoraEntrada);

        var tempoPermanencia = dataHoraSaida - registro.DataHoraEntrada;
        var valorPago = _calculadoraTarifa.Calcular(tempoPermanencia, tabelaVigente);

        registro.RegistrarSaida(dataHoraSaida, valorPago);
        await _registroRepositorio.SalvarAsync();
    }
}
