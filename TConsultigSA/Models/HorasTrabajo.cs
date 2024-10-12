using System;
using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class HorasTrabajo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El empleado es obligatorio")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora de entrada es obligatoria")]
        public DateTime HoraEntrada { get; set; }

        public DateTime? HoraSalida { get; set; }  // Campo opcional

        public decimal? TotalHoras { get; set; }  // Campo opcional

        [MaxLength(255)]
        public string Observaciones { get; set; }

        public bool Aprobado { get; set; } = false;

        // Propiedad de navegación al empleado (opcional si se desea acceder a los detalles del empleado)
        public Empleado Empleado { get; set; }
    }
}
