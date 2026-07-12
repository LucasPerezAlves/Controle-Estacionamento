using System;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class TabelaPrecoRepositorioTests : TesteComBancoEmMemoria
{
    [Fact]
    public void ObterTodas_QuandoExisteUmaTabelaCadastrada_DeveRetornarEla()
    {
        var repositorio = new TabelaPrecoRepositorio(Contexto);
        var tabela = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };

        repositorio.Adicionar(tabela);
        repositorio.Salvar();

        var tabelas = repositorio.ObterTodas();

        tabelas.Should().ContainSingle(t => t.ValorHoraInicial == 2.00m && t.ValorHoraAdicional == 1.00m);
    }
}
