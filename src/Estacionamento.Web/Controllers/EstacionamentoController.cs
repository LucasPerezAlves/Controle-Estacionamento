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
        if (string.IsNullOrWhiteSpace(placa))
        {
            TempData["Erro"] = "Informe uma placa valida.";
            return RedirectToAction(nameof(Index));
        }

        if (_registroRepositorio.BuscarAbertoPorPlaca(placa) is not null)
        {
            TempData["Erro"] = $"Ja existe um registro aberto para a placa {placa}.";
            return RedirectToAction(nameof(Index));
        }

        var registro = new RegistroEstacionamento(placa, DateTime.Now);
        _registroRepositorio.Adicionar(registro);
        _registroRepositorio.Salvar();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult MarcarSaida(string placa)
    {
        try
        {
            _registradorDeSaida.Registrar(placa, DateTime.Now);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Erro"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

}
