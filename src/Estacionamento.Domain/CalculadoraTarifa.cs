using System;

namespace Estacionamento.Domain;

public class CalculadoraTarifa
{
    public decimal Calcular(TimeSpan tempoPermanencia, TabelaPreco tabelaPreco)
    {
        if(tempoPermanencia <= TimeSpan.FromMinutes(30))
        {
            return tabelaPreco.ValorHoraInicial / 2;
        }

         if(tempoPermanencia <= TimeSpan.FromHours(1))
        {
            return tabelaPreco.ValorHoraInicial;
        }

        throw new NotImplementedException();
    }
}