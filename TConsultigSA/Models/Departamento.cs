using System.ComponentModel.DataAnnotations.Schema;

namespace TConsultigSA.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        public string DepartamentoNombre { get; set; }
        public int? IdLider { get; set; }  // Nullable en caso de no seleccionar líder
        public int IdEmpresa { get; set; }
    }
}
