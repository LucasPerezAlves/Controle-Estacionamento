using System;

namespace Estacionamento.Domain;

public class CalculadoraTarifa
{

    private static readonly TimeSpan TempoLimiteMeiaHora = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan DuracaoHora = TimeSpan.FromHours(1);
    private static readonly TimeSpan ToleranciaAdicional = TimeSpan.FromMinutes(10);


    public decimal Calcular(TimeSpan tempoPermanencia, TabelaPreco tabelaPreco)
    {
        if(tempoPermanencia <= TempoLimiteMeiaHora)
        {
            return tabelaPreco.ValorHoraInicial / 2;
        }

         if(tempoPermanencia <= DuracaoHora + ToleranciaAdicional)
        {
            return tabelaPreco.ValorHoraInicial;
        }

        var tempoAlemDaPrimeiraHora = tempoPermanencia - DuracaoHora;
        var horasAdicionais = Math.Ceiling((tempoAlemDaPrimeiraHora - ToleranciaAdicional).TotalMinutes / DuracaoHora.TotalMinutes);

        return tabelaPreco.ValorHoraInicial + (decimal)horasAdicionais * tabelaPreco.ValorHoraAdicional;
    }
}