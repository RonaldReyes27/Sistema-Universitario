using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaUniversitario.Dominio.Entidades;

namespace SistemaUniversitario.Aplicacion
{
    /// <summary>
    /// Gestiona las matrículas de los estudiantes, sus calificaciones y reportes.
    /// Incluye consultas analíticas con LINQ y validaciones básicas.
    /// </summary>
    public class GestorMatriculas
    {
        private readonly List<Matricula> matriculas = new();

        /// <summary>
        /// Matricula un estudiante en un curso, validando que no esté repetido.
        /// </summary>
        public void MatricularEstudiante(Estudiante estudiante, Curso curso)
        {
            if (estudiante == null) throw new ArgumentNullException(nameof(estudiante));
            if (curso == null) throw new ArgumentNullException(nameof(curso));

            if (matriculas.Any(m => m.Estudiante.Identificacion == estudiante.Identificacion &&
                                    m.Curso.Codigo == curso.Codigo))
                throw new InvalidOperationException($"El estudiante {estudiante.Nombre} ya está matriculado en {curso.Nombre}");

            matriculas.Add(new Matricula(estudiante, curso));
        }

        /// <summary>
        /// Agrega una calificación (0 a 10) a una matrícula existente.
        /// </summary>
        public void AgregarCalificacion(string idEstudiante, string codigoCurso, decimal calificacion)
        {
            if (calificacion < 0 || calificacion > 10)
                throw new ArgumentException("La calificación debe estar entre 0 y 10");

            var matricula = matriculas.FirstOrDefault(m =>
                m.Estudiante.Identificacion == idEstudiante &&
                m.Curso.Codigo == codigoCurso);

            if (matricula == null)
                throw new InvalidOperationException("No se encontró la matrícula especificada");

            matricula.AgregarCalificacion(calificacion);
        }

        /// <summary>
        /// Devuelve todas las matrículas de un estudiante.
        /// </summary>
        public List<Matricula> ObtenerMatriculasPorEstudiante(string idEstudiante)
            => matriculas.Where(m => m.Estudiante.Identificacion == idEstudiante).ToList();

        /// <summary>
        /// Devuelve todas las matrículas asociadas a un curso.
        /// </summary>
        public List<Matricula> ObtenerEstudiantesPorCurso(string codigoCurso)
            => matriculas.Where(m => m.Curso.Codigo == codigoCurso).ToList();

        /// <summary>
        /// Genera un reporte detallado en formato texto para un estudiante.
        /// </summary>
        public string GenerarReporteEstudiante(string idEstudiante)
        {
            var mats = ObtenerMatriculasPorEstudiante(idEstudiante);
            if (mats.Count == 0)
                return "No se encontraron matrículas para este estudiante";

            var estudiante = mats.First().Estudiante;
            var sb = new StringBuilder();

            sb.AppendLine($"\n===== REPORTE DE {estudiante.Nombre} {estudiante.Apellido} =====");
            sb.AppendLine($"ID: {estudiante.Identificacion}");
            sb.AppendLine($"Carrera: {estudiante.Carrera}");
            sb.AppendLine($"Matrícula: {estudiante.NumeroMatricula}");
            sb.AppendLine($"\nCursos Matriculados: {mats.Count}");
            sb.AppendLine("\nDetalle de Cursos:");

            foreach (var m in mats)
            {
                sb.AppendLine($"  - {m.Curso.Nombre} ({m.Curso.Codigo})");
                sb.AppendLine($"    Calificaciones: {(m.Calificaciones.Count > 0 ? string.Join(\", \", m.Calificaciones) : \"—\")}");
                sb.AppendLine($"    Promedio: {m.ObtenerPromedio():F2}");
                sb.AppendLine($"    Estado: {m.ObtenerEstado()}");
            }

            var conNotas = mats.Where(x => x.Calificaciones.Count > 0).ToList();
            var promedioGeneral = conNotas.Count == 0 ? 0 : conNotas.Average(x => x.ObtenerPromedio());

            sb.AppendLine($"\nPromedio General: {promedioGeneral:F2}");
            return sb.ToString();
        }

        // ====================== CONSULTAS LINQ ======================

        /// <summary>
        /// Devuelve los 10 estudiantes con mejor promedio general.
        /// </summary>
        public List<Estudiante> ObtenerTop10Estudiantes()
        {
            return matriculas
                .GroupBy(m => m.Estudiante)
                .Select(g => new
                {
                    Estudiante = g.Key,
                    Promedio = g.Where(m => m.Calificaciones.Count > 0)
                                .Average(m => m.ObtenerPromedio())
                })
                .OrderByDescending(x => x.Promedio)
                .Take(10)
                .Select(x => x.Estudiante)
                .ToList();
        }

        /// <summary>
        /// Devuelve los estudiantes con promedio general menor a 7.0.
        /// </summary>
        public List<Estudiante> ObtenerEstudiantesEnRiesgo()
        {
            return matriculas
                .GroupBy(m => m.Estudiante)
                .Where(g => g.Any(m => m.Calificaciones.Count > 0))
                .Select(g => new
                {
                    Estudiante = g.Key,
                    Promedio = g.Average(m => m.ObtenerPromedio())
                })
                .Where(x => x.Promedio < 7.0m)
                .Select(x => x.Estudiante)
                .ToList();
        }

        /// <summary>
        /// Devuelve los cursos con más estudiantes matriculados.
        /// </summary>
        public List<Curso> ObtenerCursosMasPopulares()
        {
            return matriculas
                .GroupBy(m => m.Curso)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToList();
        }

        /// <summary>
        /// Calcula el promedio general de todas las matrículas.
        /// </summary>
        public decimal ObtenerPromedioGeneral()
        {
            var conNotas = matriculas.Where(m => m.Calificaciones.Count > 0).ToList();
            return conNotas.Count == 0 ? 0 : conNotas.Average(m => m.ObtenerPromedio());
        }

        /// <summary>
        /// Genera estadísticas agrupadas por carrera.
        /// </summary>
        public Dictionary<string, object> ObtenerEstadisticasPorCarrera()
        {
            return matriculas
                .GroupBy(m => m.Estudiante.Carrera)
                .ToDictionary(
                    g => g.Key,
                    g => (object)new
                    {
                        Cantidad = g.Select(m => m.Estudiante.Identificacion).Distinct().Count(),
                        PromedioGeneral = g.Where(m => m.Calificaciones.Count > 0)
                                           .Average(m => m.ObtenerPromedio())
                    }
                );
        }

        /// <summary>
        /// Permite buscar estudiantes según una condición lambda.
        /// </summary>
        public List<Estudiante> BuscarEstudiantes(Func<Estudiante, bool> criterio)
        {
            return matriculas
                .Select(m => m.Estudiante)
                .Distinct()
                .Where(criterio)
                .ToList();
        }

        /// <summary>
        /// Devuelve una copia de todas las matrículas registradas.
        /// </summary>
        public List<Matricula> ObtenerTodasMatriculas() => matriculas.ToList();
    }
}
