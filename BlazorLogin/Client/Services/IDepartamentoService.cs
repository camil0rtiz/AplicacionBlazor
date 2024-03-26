using BlazorLogin.Shared;

namespace BlazorLogin.Client.Services
{
    public interface IDepartamentoService
    {
        Task<List<DepartamentoDTO>> Lista();
    }
}
