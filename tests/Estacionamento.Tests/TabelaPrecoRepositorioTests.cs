using System;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class TabelaPrecoRepositorioTests : TesteComBancoEmMemoria
{
    [Fact]
    public async Task ObterTodas_QuandoExisteUmaTabelaCadastrada_DeveRetornarEla()
    {
        var repositorio = new TabelaPrecoRepositorio(Contexto);
        var tabela = new TabelaPreco(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), 2.00m, 1.00m);

        repositorio.Adicionar(tabela);
        await repositorio.SalvarAsync();

        var tabelas = await repositorio.ObterTodasAsync();

        tabelas.Should().ContainSingle(t => t.ValorHoraInicial == 2.00m && t.ValorHoraAdicional == 1.00m);
    }
}
