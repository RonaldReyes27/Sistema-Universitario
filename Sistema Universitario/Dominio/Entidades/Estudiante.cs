using System;
using SistemaUniversitario.Aplicacion.Validacion;

namespace SistemaUniversitario.Dominio.Entidades
{
    /// <summary>
    /// Representa a un estudiante dentro del sistema universitario.
    /// Hereda de Persona e incluye información académica adicional.
    /// </summary>
    public class Estudiante : Persona
    {
        private string carrera = "";
        private string numeroMatricula = "";

        [Requerido]
        public string Carrera
        {
            get => carrera;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La carrera no puede estar vacía");
                carrera = value;
            }
        }

        [Requerido]
        [Formato("XXX-XXXXX")]
        public string NumeroMatricula
        {
            get => numeroMatricula;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El número de matrícula no puede estar vacío");
                numeroMatricula = value;
            }
        }

        // Constructor por defecto
        public Estudiante() { }

        // Constructor completo con validaciones
        public Estudiante(string id, string nombre, string apellido, DateTime fechaNacimiento,
                          string carrera, string numeroMatricula)
        {
            if (DateTime.Today.Year - fechaNacimiento.Year < 15)
                throw new ArgumentException("El estudiante debe tener al menos 15 años");

            Identificacion = id;
            Nombre = nombre;
            Apellido = apellido;
            FechaNacimiento = fechaNacimiento;
            Carrera = carrera;
            NumeroMatricula = numeroMatricula;
        }

        public override string ObtenerRol() => "Estudiante";
    }
}
