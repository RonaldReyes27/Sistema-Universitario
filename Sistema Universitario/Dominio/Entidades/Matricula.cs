using Sistema_Universitario.Dominio.Entidades.Interfaces;
using Sistema_Universitario.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaUniversitario.Dominio.Entidades
{
    /// <summary>
    /// Representa una matrícula entre un estudiante y un curso.
    /// Permite registrar calificaciones, calcular promedios y estado académico.
    /// </summary>
    public class Matricula : IEvaluable
    {
        public Estudiante Estudiante { get; set; }
        public Curso Curso { get; set; }
        public DateTime FechaMatricula { get; set; }
        public List<decimal> Calificaciones { get; private set; }

        // Constructor principal
        public Matricula(Estudiante estudiante, Curso curso)
        {
            Estudiante = estudiante ?? throw new ArgumentNullException(nameof(estudiante));
            Curso = curso ?? throw new ArgumentNullException(nameof(curso));
            FechaMatricula = DateTime.Now;
            Calificaciones = new List<decimal>();
        }

        // Agrega una calificación con validación
        public void AgregarCalificacion(decimal calificacion)
        {
            if (calificacion < 0 || calificacion > 10)
                throw new ArgumentException("La calificación debe estar entre 0 y 10");

            Calificaciones.Add(calificacion);
        }

        // Calcula el promedio de calificaciones
        public decimal ObtenerPromedio()
        {
            if (Calificaciones.Count == 0)
                return 0;
            return Calificaciones.Average();
        }

        // Determina si el estudiante ha aprobado el curso
        public bool HaAprobado() => ObtenerPromedio() >= 7.0m;

        // Devuelve el estado de la matrícula según las calificaciones
        public string ObtenerEstado()
        {
            if (Calificaciones.Count == 0)
                return "En Curso";
            return HaAprobado() ? "Aprobado" : "Reprobado";
        }

        // Representación textual de la matrícula
        public override string ToString()
        {
            return $"{Estudiante.Nombre} {Estudiante.Apellido} - {Curso.Nombre} - " +
                   $"Promedio: {ObtenerPromedio():F2} - Estado: {ObtenerEstado()}";
        }
    }
}
