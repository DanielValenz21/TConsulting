using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class HorasTrabajo
    {
        public int Id { get; set; }

        // Aquí definimos la relación con Empleado
        public int IdEmpleado { get; set; }

        // Si no necesitas la relación de navegación, puedes quitarla o hacerla nullable
        public virtual Empleado? Empleado { get; set; }

        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Las horas trabajadas son obligatorias.")]
        public decimal TotalHoras { get; set; }

        [Required(ErrorMessage = "Las observaciones son requeridas.")]
        public string Observaciones { get; set; }

        public bool Aprobado { get; set; }
    }
}
