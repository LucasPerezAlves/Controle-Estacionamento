using System;

namespace Estacionamento.Domain;

public class RegistroEstacionamento
{
    public string Placa { get; }
    public DateTime DataHoraEntrada { get; }
    public DateTime? DataHoraSaida { get; private set; }
    public decimal? ValorPago { get; private set; }
    public bool EstaAberto => DataHoraSaida == null;

    public RegistroEstacionamento(string placa, DateTime dataHoraEntrada)
    {
        Placa = placa;
        DataHoraEntrada = dataHoraEntrada;
    }

    public void RegistrarSaida(DateTime dataHoraSaida, decimal valorPago)
    {

        if (!EstaAberto)
        {
            throw new InvalidOperationException("Este registro ja foi encerrado.");
        }

        if (dataHoraSaida < DataHoraEntrada)
        {
            throw new ArgumentException("Data de saida não pode ser anterior a data de entrada", nameof(dataHoraSaida));
        }

        DataHoraSaida = dataHoraSaida;
        ValorPago = valorPago;
    }
}
