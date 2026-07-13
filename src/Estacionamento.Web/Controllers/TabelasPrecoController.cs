using System;
using Estacionamento.Web.Data;
using Estacionamento.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Web.Controllers;

public class TabelasPrecoController : Controller
{
    private readonly TabelaPrecoRepositorio _tabelaPrecoRepositorio;

    public TabelasPrecoController(TabelaPrecoRepositorio tabelaPrecoRepositorio)
    {
        _tabelaPrecoRepositorio = tabelaPrecoRepositorio;
    }

    public IActionResult Index()
    {
        var tabelas = _tabelaPrecoRepositorio.ObterTodas();
        return View(tabelas);
    }

    [HttpPost]
    public IActionResult Cadastrar(DateTime dataInicioVigencia, DateTime dataFimVigencia, decimal valorHoraInicial, decimal valorHoraAdicional)
    {
        if (dataFimVigencia < dataInicioVigencia)
        {
            TempData["Erro"] = "A data de fim da vigencia nao pode ser anterior a data de inicio.";
            return RedirectToAction(nameof(Index));
        }

        if (valorHoraInicial < 0 || valorHoraAdicional < 0)
        {
            TempData["Erro"] = "Os valores da hora inicial e adicional nao podem ser negativos.";
            return RedirectToAction(nameof(Index));
        }

        var existeSobreposicao = _tabelaPrecoRepositorio.ObterTodas()
            .Any(t => dataInicioVigencia <= t.DataFimVigencia && dataFimVigencia >= t.DataInicioVigencia);

        if (existeSobreposicao)
        {
            TempData["Erro"] = "Ja existe uma tabela de preco vigente nesse periodo.";
            return RedirectToAction(nameof(Index));
        }

        var tabela = new TabelaPreco
        {
            DataInicioVigencia = dataInicioVigencia,
            DataFimVigencia = dataFimVigencia,
            ValorHoraInicial = valorHoraInicial,
            ValorHoraAdicional = valorHoraAdicional
        };

        _tabelaPrecoRepositorio.Adicionar(tabela);
        _tabelaPrecoRepositorio.Salvar();

        return RedirectToAction(nameof(Index));
    }

}
