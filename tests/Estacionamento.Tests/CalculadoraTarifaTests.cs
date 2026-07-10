using System;
using Estacionamento.Domain;
using Xunit;

namespace Estacionamento.Tests;

public class CalculadoraTarifaTests
{

    private readonly TabelaPreco _tabelaPreco;
    private readonly CalculadoraTarifa _calculadora;

    public CalculadoraTarifaTests()
    {
        _tabelaPreco = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m  
        };

        _calculadora = new CalculadoraTarifa();
    }

    [Fact]
    public void Calcular_PermanenciaExatosTrintaMinutos_CobrarMetadeHoraInicial()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromMinutes(30), _tabelaPreco);

        Assert.Equal(1.00m, valorPagar);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraExata_CobrarValorCheioHoraInicial()
    {
       var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(1), _tabelaPreco);

        Assert.Equal(2.00m, valorPagar);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraDezMinutos_CobrarValorCheioHoraInicial()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(10), _tabelaPreco);

        Assert.Equal(2.00m, valorPagar);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraQuinzeMinutos_CobrarHoraInicialMaisUmaHoraAdicional()
    {
       var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(15), _tabelaPreco);

        Assert.Equal(3.00m, valorPagar); 
    }

    [Fact]
    public void Calcular_PermanenciaDuasHorasCincoMinutos_CobrarHoraInicialMaisUmaHoraAdicional()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(5), _tabelaPreco);

        Assert.Equal(3.00m, valorPagar); 
    }

    [Fact]
    public void Calcular_PermanenciaDuasHorasQuinzeMinutos_CobrarHoraInicialMaisDuasHorasAdicionais()
    {
        var valorPagar = _calculadora.Calcular(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(15), _tabelaPreco);

        Assert.Equal(4.00m, valorPagar); 
    }
}