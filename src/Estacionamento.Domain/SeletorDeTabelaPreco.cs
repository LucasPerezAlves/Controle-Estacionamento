using System;
using System.Collections.Generic;

namespace Estacionamento.Domain;

public class SeletorDeTabelaPreco
{
    public TabelaPreco Selecionar(IEnumerable<TabelaPreco> tabelas, DateTime dataReferencia)
    {
        return tabelas.Single(t => t.DataInicioVigencia <= dataReferencia && t.DataFimVigencia >= dataReferencia);
    }
}