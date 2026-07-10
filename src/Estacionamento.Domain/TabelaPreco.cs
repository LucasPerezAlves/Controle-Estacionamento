using System;

namespace Estacionamento.Domain;

public class TabelaPreco
{
    public DateTime DataInicioVigencia { get; set; }
    public DateTime DataFimVigencia { get; set; }
    public decimal ValorHoraInicial { get; set; }
    public decimal ValorHoraAdicional { get; set; }
}