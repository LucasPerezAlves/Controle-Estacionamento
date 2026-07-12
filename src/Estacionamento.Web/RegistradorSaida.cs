using System;
using Estacionamento.Domain;
using Estacionamento.Web.Data;

namespace Estacionamento.Web.Services;

public class RegistradorDeSaida
{
    private readonly RegistroEstacionamentoRepositorio _registroRepositorio;
    private readonly TabelaPrecoRepositorio _tabelaPrecoRepositorio;
    private readonly SeletorDeTabelaPreco _seletorDeTabelaPreco;
    private readonly CalculadoraTarifa _calculadoraTarifa;

    public RegistradorDeSaida(
        RegistroEstacionamentoRepositorio registroRepositorio,
        TabelaPrecoRepositorio tabelaPrecoRepositorio,
        SeletorDeTabelaPreco seletorDeTabelaPreco,
        CalculadoraTarifa calculadoraTarifa)
    {
        _registroRepositorio = registroRepositorio;
        _tabelaPrecoRepositorio = tabelaPrecoRepositorio;
        _seletorDeTabelaPreco = seletorDeTabelaPreco;
        _calculadoraTarifa = calculadoraTarifa;
    }

    public void Registrar(string placa, DateTime dataHoraSaida)
    {
        var registro = _registroRepositorio.BuscarAbertoPorPlaca(placa);

        if (registro is null)
        {
          throw new InvalidOperationException($"Nao existe um registro aberto para a placa {placa}.");  
        }

        var tabelas = _tabelaPrecoRepositorio.ObterTodas();
        var tabelaVigente = _seletorDeTabelaPreco.Selecionar(tabelas, registro.DataHoraEntrada);

        var tempoPermanencia = dataHoraSaida - registro.DataHoraEntrada;
        var valorPago = _calculadoraTarifa.Calcular(tempoPermanencia, tabelaVigente);

        registro.RegistrarSaida(dataHoraSaida, valorPago);
        _registroRepositorio.Salvar();
    }
}
