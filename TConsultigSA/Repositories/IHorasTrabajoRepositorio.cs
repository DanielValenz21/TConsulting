using TConsultigSA.Models;

namespace TConsultingSA.Repositories
{
    public interface IHorasTrabajoRepositorio
    {
        Task<IEnumerable<HorasTrabajo>> GetAll();
        Task<int> Add(HorasTrabajo horasTrabajo);
        // Puedes agregar más métodos según sea necesario, como Update, Delete, etc.
    }
}
