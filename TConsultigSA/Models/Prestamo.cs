namespace TConsultigSA.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int IdEmpleado { get; set; }
        public decimal Total { get; set; }
        public int CuotasPendientes { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public int IdTipo { get; set; }  // Asegúrate de que este campo esté presente
    }

}
