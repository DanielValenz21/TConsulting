using System;
using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class HistorialAumento
    {
        public int Id { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        public decimal PorcentajeAumento { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "La cantidad de aumento debe ser un valor positivo.")]
        public decimal CantidadAumento { get; set; }

        [Required]
        public decimal SalarioFinal { get; set; }
    }
}
