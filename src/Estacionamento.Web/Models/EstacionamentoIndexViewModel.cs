using Estacionamento.Domain;

namespace Estacionamento.Web.Models;

public class EstacionamentoIndexViewModel
{
    public IReadOnlyList<RegistroEstacionamento> Abertos { get; init; } = Array.Empty<RegistroEstacionamento>();
    public IReadOnlyList<RegistroEstacionamento> Historico { get; init; } = Array.Empty<RegistroEstacionamento>();
    public int PaginaAtual { get; init; }
    public int TotalPaginas { get; init; }
}
