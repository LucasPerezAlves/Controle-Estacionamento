using System;
using System.Collections.Generic;

namespace Estacionamento.Domain;

public class SeletorDeTabelaPreco
{
    public TabelaPreco Selecionar(IEnumerable<TabelaPreco> tabelas, DateTime dataReferencia)
    {
        var tabela = tabelas.SingleOrDefault(t => t.DataInicioVigencia <= dataReferencia && t.DataFimVigencia >= dataReferencia);

        if (tabela is null)
        {
            throw new InvalidOperationException($"Nao existe tabela de preco vigente para a data {dataReferencia:dd/MM/yyyy}.");
        }

        return tabela;
    }
}