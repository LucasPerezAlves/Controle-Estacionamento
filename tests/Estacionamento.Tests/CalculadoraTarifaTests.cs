using System;
using Estacionamento.Domain;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class CalculadoraTarifaTests
{
    private readonly TabelaPreco _tabelaPreco;
    private readonly CalculadoraTarifa _calculadora;

    public CalculadoraTarifaTests()
    {
        _tabelaPreco = new TabelaPreco(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), 2.00m, 1.00m);

        _calculadora = new CalculadoraTarifa();
    }

    [Fact]
    public void Calcular_PermanenciaExatosTrintaMinutos_CobrarMetadeHoraInicial()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromMinutes(30), _tabelaPreco);

        valorPagar.Should().Be(1.00m);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraExata_CobrarValorCheioHoraInicial()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(1), _tabelaPreco);

        valorPagar.Should().Be(2.00m);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraDezMinutos_CobrarValorCheioHoraInicial()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(10), _tabelaPreco);

        valorPagar.Should().Be(2.00m);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraQuinzeMinutos_CobrarHoraInicialMaisUmaHoraAdicional()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(15), _tabelaPreco);

        valorPagar.Should().Be(3.00m);
    }

    [Fact]
    public void Calcular_PermanenciaDuasHorasCincoMinutos_CobrarHoraInicialMaisUmaHoraAdicional()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(5), _tabelaPreco);

        valorPagar.Should().Be(3.00m);
    }

    [Fact]
    public void Calcular_PermanenciaDuasHorasQuinzeMinutos_CobrarHoraInicialMaisDuasHorasAdicionais()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(15), _tabelaPreco);

        valorPagar.Should().Be(4.00m);
    }
}
