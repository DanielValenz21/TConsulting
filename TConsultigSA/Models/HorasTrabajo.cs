using System;
using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class HorasTrabajo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El IdEmpleado es obligatorio")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "La Fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La Hora de Entrada es obligatoria")]
        public DateTime HoraEntrada { get; set; }

        public DateTime? HoraSalida { get; set; }  // Es opcional
        public decimal? TotalHoras { get; set; }  // Aquí usas TotalHoras en lugar de Horas

        [MaxLength(255)]
        public string Observaciones { get; set; }

        public bool Aprobado { get; set; }

    }
}
