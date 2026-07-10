using System;
using Estacionamento.Domain;
using Xunit;

namespace Estacionamento.Tests;

public class CalculadoraTarifaTests
{
    [Fact]
    public void Calcular_PermanenciaExatosTrintaMinutos_CobrarMetadeHoraInicial()
    {
        var tabelaPreco = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };

        var calculadora = new CalculadoraTarifa();

        var valorPagar = calculadora.Calcular(TimeSpan.FromMinutes(30), tabelaPreco);

        Assert.Equal(1.00m, valorPagar);
    }

    [Fact]
    public void Calcular_PermanenciaUmaHoraExata_CobrarValorCheioHoraInicial()
    {
        var tabelaPreco = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m 
        };

        var calculadora = new CalculadoraTarifa();

        var valorPagar = calculadora.Calcular(TimeSpan.FromHours(1), tabelaPreco);

        Assert.Equal(2.00m, valorPagar);
    }
}