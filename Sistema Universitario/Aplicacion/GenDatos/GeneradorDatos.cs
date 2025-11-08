using System;
using System.Collections.Generic;
using System.Linq;
using Sistema_Universitario.Aplicacion;
using Sistema_Universitario.Aplicacion.Utils;
using SistemaUniversitario.Dominio.Entidades;
using SistemaUniversitario.Dominio.Enums;


namespace SistemaUniversitario.Aplicacion.GenDatos
{

    /// Genera datos de prueba (profesores, estudiantes, cursos y matrículas)
    /// para demostrar las funcionalidades del sistema.
    
    public class GeneradorDatos
    {
        private static readonly Random random = new();

       
        /// Crea datos simulados de profesores, cursos y estudiantes,
        /// y luego matricula automáticamente a los estudiantes.
      
        public void GenerarDatosPrueba(
            Repositorio<Estudiante> repoEstudiantes,
            Repositorio<Profesor> repoProfesores,
            Repositorio<Curso> repoCursos,
            GestorMatriculas gestorMatriculas)
        {
            Console.WriteLine("Generando datos de prueba...\n");

            // ========== Profesores ==========
            var profesores = new List<Profesor>
            {
                new("P001", "Ivan", "Zorrilla", new DateTime(1980, 5, 15), "Ingeniería", TipoContrato.TiempoCompleto, 3500),
                new("P002", "Jose", "Luis", new DateTime(1985, 8, 22), "Ciencias", TipoContrato.TiempoCompleto, 3200),
                new("P003", "Edwin", "Fria", new DateTime(1978, 3, 10), "Matemáticas", TipoContrato.TiempoCompleto, 3800),
                new("P004", "Victor", "Rijo", new DateTime(1982, 11, 5), "Sistemas", TipoContrato.MedioTiempo, 2000),
                new("P005", "Pedro", "Sánchez", new DateTime(1975, 7, 18), "Negocios", TipoContrato.TiempoCompleto, 4000)
            };

            profesores.ForEach(repoProfesores.Agregar);

            // ========== Cursos ==========
            var cursos = new List<Curso>
            {
                new("CS101", "Programación I", 4, profesores[0]),
                new("CS102", "Programación II", 4, profesores[0]),
                new("CS201", "Estructuras de Datos", 5, profesores[3]),
                new("CS202", "Algoritmos", 5, profesores[3]),
                new("MAT101", "Cálculo I", 4, profesores[2]),
                new("MAT102", "Cálculo II", 4, profesores[2]),
                new("FIS101", "Física I", 4, profesores[1]),
                new("ADM101", "Administración", 3, profesores[4]),
                new("CS301", "Bases de Datos", 5, profesores[0]),
                new("CS302", "Redes", 4, profesores[3])
            };

            cursos.ForEach(repoCursos.Agregar);

            // ========== Estudiantes ==========
            string[] carreras = { "Ingeniería en Software", "Ingeniería Industrial", "Administración", "Contabilidad" };
            string[] nombres = { "Luis", "Elian", "Tomas", "Ramon", "Dubenny", "Neiry", "Joseidi", "Jeifferson", "Braylin", "Angel", "Andrea", "Ronald", "Ricardo", "Mónica", "Javier" };
            string[] apellidos = { "García", "Pérez", "Ramírez", "Torres", "Flores", "Rivera", "Gómez", "Díaz", "Cruz", "Morales", "Jiménez", "Hernández", "Ruiz", "Mendoza", "Castro" };

            for (int i = 0; i < 15; i++)
            {
                var estudiante = new Estudiante(
                    $"E{i + 1:D3}",
                    nombres[i],
                    apellidos[i],
                    new DateTime(2000 + random.Next(0, 5), random.Next(1, 13), random.Next(1, 28)),
                    carreras[random.Next(carreras.Length)],
                    $"{random.Next(100, 1000)}-{random.Next(10000, 100000)}"
                );

                repoEstudiantes.Agregar(estudiante);
            }

            // ========== Matrículas ==========
            var estudiantes = repoEstudiantes.ObtenerTodos();
            int matriculasCreadas = 0;

            foreach (var estudiante in estudiantes)
            {
                // Cada estudiante se matricula en entre 2 y 4 cursos
                int numCursos = random.Next(2, 5);
                var cursosSeleccionados = cursos.OrderBy(_ => random.Next()).Take(numCursos).ToList();

                foreach (var curso in cursosSeleccionados)
                {
                    try
                    {
                        gestorMatriculas.MatricularEstudiante(estudiante, curso);
                        matriculasCreadas++;

                        // Cada matrícula obtiene 3 o 4 calificaciones
                        int numNotas = random.Next(3, 5);
                        for (int j = 0; j < numNotas; j++)
                        {
                            decimal calificacion = Math.Round((decimal)(random.NextDouble() * 4 + 6), 1); // entre 6.0 y 10.0
                            gestorMatriculas.AgregarCalificacion(estudiante.Identificacion, curso.Codigo, calificacion);
                        }
                    }
                    catch
                    {
                        // Ignorar duplicados
                    }
                }
            }

            Console.WriteLine($"✓ {profesores.Count} profesores agregados");
            Console.WriteLine($"✓ {cursos.Count} cursos agregados");
            Console.WriteLine($"✓ {estudiantes.Count} estudiantes agregados");
            Console.WriteLine($"✓ {matriculasCreadas} matrículas creadas\n");
            Console.WriteLine("¡Datos de prueba generados exitosamente!\n");
        }

        
        /// Ejecuta una demostración completa del sistema mostrando datos, promedios y análisis.
        
        public void DemostrarFuncionalidades(
            Repositorio<Estudiante> repoEstudiantes,
            Repositorio<Profesor> repoProfesores,
            Repositorio<Curso> repoCursos,
            GestorMatriculas gestorMatriculas)
        {
            Console.WriteLine("\n================ DEMOSTRACIÓN DE FUNCIONALIDADES ================\n");

            // 1️⃣ Top 10 Estudiantes
            Console.WriteLine("1️⃣ Top 10 Estudiantes:");
            var top = gestorMatriculas.ObtenerTop10Estudiantes();
            foreach (var e in top.Take(5))
            {
                var mats = gestorMatriculas.ObtenerMatriculasPorEstudiante(e.Identificacion);
                var prom = mats.Where(m => m.Calificaciones.Count > 0).Average(m => m.ObtenerPromedio());
                Console.WriteLine($"   {e.Nombre} {e.Apellido} → Promedio: {prom:F2}");
            }

            // 2️⃣ Estudiantes en riesgo
            Console.WriteLine($"\n2️⃣ Estudiantes en riesgo (<7.0): {gestorMatriculas.ObtenerEstudiantesEnRiesgo().Count}");

            // 3️⃣ Cursos más populares
            Console.WriteLine("\n3️⃣ Cursos más populares:");
            var populares = gestorMatriculas.ObtenerCursosMasPopulares().Take(5);
            foreach (var c in populares)
            {
                int cantidad = gestorMatriculas.ObtenerEstudiantesPorCurso(c.Codigo).Count;
                Console.WriteLine($"   {c.Nombre} → {cantidad} estudiantes");
            }

            // 4️⃣ Promedio general
            Console.WriteLine($"\n4️⃣ Promedio general del sistema: {gestorMatriculas.ObtenerPromedioGeneral():F2}");

            // 5️⃣ Estadísticas por carrera
            Console.WriteLine("\n5️⃣ Estadísticas por carrera:");
            var stats = gestorMatriculas.ObtenerEstadisticasPorCarrera();
            foreach (var kv in stats)
            {
                dynamic valores = kv.Value;
                Console.WriteLine($"   {kv.Key}: {valores.Cantidad} estudiantes → Promedio: {valores.PromedioGeneral:F2}");
            }

            Console.WriteLine("\n===============================================================\n");
        }
    }
}
