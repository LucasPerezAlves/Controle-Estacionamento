using System;
using Estacionamento.Domain;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class RegistroEstacionamentoTests
{
    private readonly DateTime _dataEntrada = new DateTime(2024, 5, 10, 8, 0, 0);
    private readonly RegistroEstacionamento _registro;

    public RegistroEstacionamentoTests()
    {
        _registro = new RegistroEstacionamento("ABC1234", _dataEntrada);
    }

    [Fact]
    public void Criar_ComPlacaEDataEntrada_DeveEstarAbertoComDadosCorretos()
    {
        _registro.Placa.Should().Be("ABC1234");
        _registro.DataHoraEntrada.Should().Be(_dataEntrada);
        _registro.EstaAberto.Should().BeTrue();
        _registro.DataHoraSaida.Should().BeNull();
        _registro.ValorPago.Should().BeNull();
    }

    [Fact]
    public void RegistrarSaida_QuandoSaidaEPosteriorAEntrada_DevePreencherDataHoraSaida()
    {
        var dataSaida = new DateTime(2024, 5, 10, 9, 30, 0);
        _registro.RegistrarSaida(dataSaida, 2.00m);

        _registro.DataHoraSaida.Should().Be(dataSaida);
        _registro.ValorPago.Should().Be(2.00m);
        _registro.EstaAberto.Should().BeFalse();
    }

    [Fact]
    public void RegistrarSaida_QuandoSaidaAnteriorAEntrada_DeveLancarExcecao()
    {
        var dataSaidaInvalida = new DateTime(2024, 5, 10, 7, 0, 0);

        var acao = () => _registro.RegistrarSaida(dataSaidaInvalida, 2.00m);

        acao.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RegistrarSaida_QuandoRegistroJaEstaFechado_DeveLancarExcecao()
    {
        _registro.RegistrarSaida(new DateTime(2024, 5, 10, 9, 0, 0), 2.00m);

        var acao = () => _registro.RegistrarSaida(new DateTime(2024, 5, 10, 10, 0, 0), 3.00m);

        acao.Should().Throw<InvalidOperationException>();
    }
}
