namespace WebAPISqlServer.Models.DTO
{
    public class AlumnoDTO
    {
        public string Matricula { get; set; } = null!;

        public string NombreAlumno { get; set; } = null!;

        public string ApellidoAlumno { get; set; } = null!;

        public int SemestreActual { get; set; }

        public string CorreoAlumno { get; set; } = null!;
    }
}
