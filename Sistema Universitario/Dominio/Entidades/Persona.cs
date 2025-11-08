using System;
using Sistema_Universitario.Dominio.Interfaces;
using SistemaUniversitario.Aplicacion.Validacion;
using SistemaUniversitario.Dominio.Entidades;
using SistemaUniversitario.Dominio.Enums;


namespace SistemaUniversitario.Dominio.Entidades
{
  
    /// Clase base abstracta para todas las personas del sistema (Estudiante, Profesor).
    /// Contiene propiedades comunes y validaciones básicas.
   
    public abstract class Persona : IIdentificable
    {
        private string identificacion = "";
        private string nombre = "";
        private string apellido = "";
        private DateTime fechaNacimiento;

        // Propiedad requerida: Identificación única
        [Requerido]
        public string Identificacion
        {
            get => identificacion;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La identificación no puede estar vacía");
                identificacion = value;
            }
        }

        // Propiedad requerida: Nombre de la persona
        [Requerido]
        public string Nombre
        {
            get => nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío");
                nombre = value;
            }
        }

        // Propiedad requerida: Apellido de la persona
        [Requerido]
        public string Apellido
        {
            get => apellido;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El apellido no puede estar vacío");
                apellido = value;
            }
        }

        // Fecha de nacimiento con validación
        public DateTime FechaNacimiento
        {
            get => fechaNacimiento;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de nacimiento no puede ser futura");
                fechaNacimiento = value;
            }
        }

        // Cálculo automático de la edad
        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                int edad = hoy.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
                return edad;
            }
        }

        // Método abstracto que define el rol (Estudiante o Profesor)
        public abstract string ObtenerRol();

        // Representación textual común
        public override string ToString()
            => $"{Nombre} {Apellido} - ID: {Identificacion} - Edad: {Edad} años - Rol: {ObtenerRol()}";
    }
}
