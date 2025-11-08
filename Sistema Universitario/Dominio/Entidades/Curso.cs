using Sistema_Universitario.Dominio.Interfaces;
using SistemaUniversitario.Aplicacion.Validacion;
using System;
using SistemaUniversitario.Dominio.Entidades;
using SistemaUniversitario.Dominio.Enums;


namespace SistemaUniversitario.Dominio.Entidades
{
    /// <summary>
    /// Representa un curso académico dentro del sistema universitario.
    /// Incluye código, nombre, créditos y el profesor asignado.
    /// </summary>
    public class Curso : IIdentificable
    {
        private string codigo = "";
        private string nombre = "";

        [Requerido]
        public string Codigo
        {
            get => codigo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException("El código no puede estar vacío");
                codigo = value;
            }
        }

        // Implementación de la interfaz IIdentificable
        string IIdentificable.Identificacion => Codigo;

        [Requerido]
        public string Nombre
        {
            get => nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException("El nombre no puede estar vacío");
                nombre = value;
            }
        }

        [ValidacionRango(1, 6)]
        public int Creditos { get; set; }

        // Profesor asignado al curso (opcional)
        public Profesor? ProfesorAsignado { get; set; }

        // Constructor por defecto
        public Curso() { }

        // Constructor completo
        public Curso(string codigo, string nombre, int creditos, Profesor? profesor = null)
        {
            Codigo = codigo;
            Nombre = nombre;
            Creditos = creditos;
            ProfesorAsignado = profesor;
        }

        // Representación textual del curso
        public override string ToString()
        {
            var prof = ProfesorAsignado != null
                ? $"{ProfesorAsignado.Nombre} {ProfesorAsignado.Apellido}"
                : "Sin asignar";
            return $"{Codigo} - {Nombre} ({Creditos} créditos) - Profesor: {prof}";
        }
    }
}
