using System;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using Estacionamento.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Web.Controllers;

public class EstacionamentoController : Controller
{
    private readonly RegistroEstacionamentoRepositorio _registroRepositorio;
    private readonly RegistradorDeSaida _registradorDeSaida;

    public EstacionamentoController(RegistroEstacionamentoRepositorio registroRepositorio, RegistradorDeSaida registradorDeSaida)
    {
        _registroRepositorio = registroRepositorio;
        _registradorDeSaida = registradorDeSaida;
    }

    public IActionResult Index()
    {
        var registros = _registroRepositorio.ObterTodos();
        return View(registros);
    }

    [HttpPost]
    public IActionResult MarcarEntrada(string placa)
    {
        var registro = new RegistroEstacionamento(placa, DateTime.Now);
        _registroRepositorio.Adicionar(registro);
        _registroRepositorio.Salvar();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult MarcarSaida(string placa)
    {
        _registradorDeSaida.Registrar(placa, DateTime.Now);

        return RedirectToAction(nameof(Index));
    }

}
