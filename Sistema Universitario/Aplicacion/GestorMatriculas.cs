using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities = SistemaUniversitario.Dominio.Entidades;
using SistemaUniversitario.Aplicacion.GenDatos;

namespace SistemaUniversitario.Aplicacion
{
    public class GestorMatriculas
    {
        private readonly List<Entities.Matricula> _matriculas = new();

        public void MatricularEstudiante(Entities.Estudiante estudiante, Entities.Curso curso)
        {
            if (estudiante == null) throw new ArgumentNullException(nameof(estudiante));
            if (curso == null) throw new ArgumentNullException(nameof(curso));

            if (_matriculas.Any(m => m.Estudiante.Identificacion == estudiante.Identificacion &&
                                     m.Curso.Codigo == curso.Codigo))
                throw new InvalidOperationException($"El estudiante {estudiante.Nombre} ya está matriculado en {curso.Nombre}");

            _matriculas.Add(new Entities.Matricula(estudiante, curso));
        }

        public void AgregarCalificacion(string idEstudiante, string codigoCurso, decimal calificacion)
        {
            if (calificacion < 0 || calificacion > 10)
                throw new ArgumentException("La calificación debe estar entre 0 y 10");

            var mat = _matriculas.FirstOrDefault(m =>
                m.Estudiante.Identificacion == idEstudiante &&
                m.Curso.Codigo == codigoCurso);

            if (mat == null)
                throw new InvalidOperationException("No se encontró la matrícula especificada");

            mat.AgregarCalificacion(calificacion);
        }

        public List<Entities.Matricula> ObtenerMatriculasPorEstudiante(string idEstudiante) =>
            _matriculas.Where(m => m.Estudiante.Identificacion == idEstudiante).ToList();

        public List<Entities.Matricula> ObtenerEstudiantesPorCurso(string codigoCurso) =>
            _matriculas.Where(m => m.Curso.Codigo == codigoCurso).ToList();

        public string GenerarReporteEstudiante(string idEstudiante)
        {
            var mats = ObtenerMatriculasPorEstudiante(idEstudiante);
            if (mats.Count == 0) return "No se encontraron matrículas para este estudiante";

            var e = mats.First().Estudiante;
            var sb = new StringBuilder();

            sb.AppendLine($"\n===== REPORTE DE {e.Nombre} {e.Apellido} =====");
            sb.AppendLine($"ID: {e.Identificacion}");
            sb.AppendLine($"Carrera: {e.Carrera}");
            sb.AppendLine($"Matrícula: {e.NumeroMatricula}");
            sb.AppendLine($"\nCursos Matriculados: {mats.Count}");
            sb.AppendLine("\nDetalle de Cursos:");

            foreach (var m in mats)
            {
                sb.AppendLine($"  - {m.Curso.Nombre} ({m.Curso.Codigo})");
                sb.AppendLine($"    Calificaciones: {string.Join(", ", m.Calificaciones)}");
                sb.AppendLine($"    Promedio: {m.ObtenerPromedio():F2}");
                sb.AppendLine($"    Estado: {m.ObtenerEstado()}");
            }

            var prom = mats.Where(m => m.Calificaciones.Count > 0).Average(m => m.ObtenerPromedio());
            sb.AppendLine($"\nPromedio General: {prom:F2}");
            return sb.ToString();
        }

        // ===== Consultas LINQ =====
        public List<Entities.Estudiante> ObtenerTop10Estudiantes() =>
            _matriculas
                .GroupBy(m => m.Estudiante)
                .Select(g => new
                {
                    Estudiante = g.Key,
                    Promedio = g.Where(m => m.Calificaciones.Count > 0).Average(m => m.ObtenerPromedio())
                })
                .OrderByDescending(x => x.Promedio)
                .Take(10)
                .Select(x => x.Estudiante)
                .ToList();

        public List<Entities.Estudiante> ObtenerEstudiantesEnRiesgo() =>
            _matriculas
                .GroupBy(m => m.Estudiante)
                .Where(g => g.Any(m => m.Calificaciones.Count > 0))
                .Select(g => new
                {
                    Estudiante = g.Key,
                    Promedio = g.Where(m => m.Calificaciones.Count > 0).Average(m => m.ObtenerPromedio())
                })
                .Where(x => x.Promedio < 7.0m)
                .Select(x => x.Estudiante)
                .ToList();

        public List<Entities.Curso> ObtenerCursosMasPopulares() =>
            _matriculas.GroupBy(m => m.Curso).OrderByDescending(g => g.Count()).Select(g => g.Key).ToList();

        public decimal ObtenerPromedioGeneral()
        {
            var conNotas = _matriculas.Where(m => m.Calificaciones.Count > 0).ToList();
            return conNotas.Count == 0 ? 0 : conNotas.Average(m => m.ObtenerPromedio());
        }

        public Dictionary<string, object> ObtenerEstadisticasPorCarrera() =>
            _matriculas
                .GroupBy(m => m.Estudiante.Carrera)
                .ToDictionary(
                    g => g.Key,
                    g => (object)new
                    {
                        Cantidad = g.Select(m => m.Estudiante.Identificacion).Distinct().Count(),
                        PromedioGeneral = g.Where(m => m.Calificaciones.Count > 0).Average(m => m.ObtenerPromedio())
                    });

        public List<Entities.Estudiante> BuscarEstudiantes(Func<Entities.Estudiante, bool> criterio) =>
            _matriculas.Select(m => m.Estudiante).Distinct().Where(criterio).ToList();

        public List<Entities.Matricula> ObtenerTodasMatriculas() => _matriculas;
    }
}
