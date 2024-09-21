namespace TConsultigSA.Models
{
    public class Usuario
    {
        public int Id { get; set; }  // Clave primaria del usuario
        public string Nombre { get; set; }  // Nombre de usuario
        public string Contrasenia { get; set; }  // Contraseña del usuario
        public int IdRol { get; set; }  // Relación con la tabla Roles (clave externa)
    }
}
