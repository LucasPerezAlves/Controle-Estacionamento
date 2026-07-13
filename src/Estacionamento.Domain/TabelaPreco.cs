using System;

namespace Estacionamento.Domain;

public class TabelaPreco
{
    public int Id { get; private set; }
    public DateTime DataInicioVigencia { get; private set; }
    public DateTime DataFimVigencia { get; private set; }
    public decimal ValorHoraInicial { get; private set; }
    public decimal ValorHoraAdicional { get; private set; }

    private TabelaPreco()
    {
    }

    public TabelaPreco(DateTime dataInicioVigencia, DateTime dataFimVigencia, decimal valorHoraInicial, decimal valorHoraAdicional)
    {
        if (dataFimVigencia < dataInicioVigencia)
        {
            throw new ArgumentException("A data de fim da vigencia nao pode ser anterior a data de inicio.", nameof(dataFimVigencia));
        }

        if (valorHoraInicial < 0 || valorHoraAdicional < 0)
        {
            throw new ArgumentException("Os valores da hora inicial e adicional nao podem ser negativos.");
        }

        DataInicioVigencia = dataInicioVigencia;
        DataFimVigencia = dataFimVigencia;
        ValorHoraInicial = valorHoraInicial;
        ValorHoraAdicional = valorHoraAdicional;
    }
}
