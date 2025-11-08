using Sistema_Universitario.Dominio.Entidades.Enums;
using SistemaUniversitario.Aplicacion.Validacion;
using SistemaUniversitario.Dominio.Entidades; // Mantienes la carpeta "Enums" en inglés
using System;

namespace SistemaUniversitario.Dominio.Entidades
{
    /// <summary>
    /// Representa a un profesor dentro del sistema universitario.
    /// Hereda de Persona e incluye datos laborales como salario y tipo de contrato.
    /// </summary>
    public class Profesor : Persona
    {
        private string departamento = "";

        [Requerido]
        public string Departamento
        {
            get => departamento;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El departamento no puede estar vacío");
                departamento = value;
            }
        }

        // Enum que define el tipo de contrato del profesor (Tiempo Completo, Medio Tiempo, Por Horas)
        public TipoContrato TipoContrato { get; set; }

        // Salario base con validación de rango (entre 500 y 10,000)
        [ValidacionRango(500, 10000)]
        public decimal SalarioBase { get; set; }

        // Constructor por defecto
        public Profesor() { }

        // Constructor completo con validaciones
        public Profesor(string id, string nombre, string apellido, DateTime fechaNacimiento,
                        string departamento, TipoContrato tipoContrato, decimal salarioBase)
        {
            if (DateTime.Today.Year - fechaNacimiento.Year < 25)
                throw new ArgumentException("El profesor debe tener al menos 25 años");

            Identificacion = id;
            Nombre = nombre;
            Apellido = apellido;
            FechaNacimiento = fechaNacimiento;
            Departamento = departamento;
            TipoContrato = tipoContrato;
            SalarioBase = salarioBase;
        }

        public override string ObtenerRol() => "Profesor";
    }
}
