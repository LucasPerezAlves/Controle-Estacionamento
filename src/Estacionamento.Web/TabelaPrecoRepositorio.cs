using System;
using System.Collections.Generic;
using Estacionamento.Domain;

namespace Estacionamento.Web.Data;

public class TabelaPrecoRepositorio
{
    private readonly EstacionamentoDbContext _contexto;

    public TabelaPrecoRepositorio(EstacionamentoDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Adicionar(TabelaPreco tabela)
    {
        _contexto.TabelasPreco.Add(tabela);
    }

    public IEnumerable<TabelaPreco> ObterTodas()
    {
        return _contexto.TabelasPreco.ToList();
    }

    public void Salvar()
    {
        _contexto.SaveChanges();
    }
}
