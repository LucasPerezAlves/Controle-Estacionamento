using Estacionamento.Domain;

namespace Estacionamento.Web.Data;

public interface IRegistroEstacionamentoRepositorio
{
    void Adicionar(RegistroEstacionamento registro);
    Task<RegistroEstacionamento?> BuscarAbertoPorPlacaAsync(string placa);
    Task<IEnumerable<RegistroEstacionamento>> ObterAbertosAsync();
    Task<IEnumerable<RegistroEstacionamento>> ObterHistoricoAsync(int pagina, int tamanhoPagina);
    Task<int> ContarTodosAsync();
    Task SalvarAsync();
}
