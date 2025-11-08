using Sistema_Universitario.Aplicacion;
using SistemaUniversitario.Aplicacion;
using SistemaUniversitario.Aplicacion.GenDatos;
using SistemaUniversitario.Dominio.Entidades;
using SistemaUniversitario.Dominio.Enums;
using SistemaUniversitario.Estructura;
// Si usas reflection:


using System;
using System.Linq;

namespace SistemaUniversitario.Presentacion
{
    public class SistemaUniversitarioApp
    {
        private readonly Repositorio<Estudiante> repoEstudiantes = new();
        private readonly Repositorio<Profesor> repoProfesores = new();
        private readonly Repositorio<Curso> repoCursos = new();

        private readonly GestorMatriculas gestorMatriculas = new();
        private readonly GeneradorDatos generadorDatos = new();

        public void Ejecutar()
        {
            MostrarBienvenida();

            Console.Write("\n¿Desea cargar datos de prueba? (S/N): ");
            if ((Console.ReadLine() ?? "").Trim().ToUpper() == "S")
                generadorDatos.GenerarDatosPrueba(repoEstudiantes, repoProfesores, repoCursos, gestorMatriculas);

            bool salir = false;
            while (!salir)
            {
                try
                {
                    MostrarMenuPrincipal();
                    var opcion = LeerOpcion();

                    switch (opcion)
                    {
                        case 1: GestionarEstudiantes(); break;
                        case 2: GestionarProfesores(); break;
                        case 3: GestionarCursos(); break;
                        case 4: MatricularEstudianteEnCurso(); break;
                        case 5: RegistrarCalificaciones(); break;
                        case 6: VerReportes(); break;
                        case 7: AnalizarConReflection(); break;
                        case 8: DemostrarFuncionalidades(); break;
                        case 9: salir = true; MostrarDespedida(); break;
                        default: MostrarError("Opción no válida"); break;
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error: {ex.Message}");
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // ====== Utilidades UI ======
        private static int LeerOpcion() => int.TryParse(Console.ReadLine(), out int x) ? x : 0;

        private static void MostrarBienvenida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════╗
║                                                          ║
║        SISTEMA DE GESTIÓN UNIVERSITARIA                  ║
║                                                          ║
║        Práctica de C# - POO Avanzado                     ║
║                                                          ║
╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        private static void MarcoTitulo(string titulo, ConsoleColor color)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine($"\n=== {titulo} ===");
            Console.ResetColor();
        }

        private static void MostrarExito(string m) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"\n✓ {m}"); Console.ResetColor(); }
        private static void MostrarError(string m) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"\n✗ {m}"); Console.ResetColor(); }
        private static void MostrarWarn(string m) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"\n⚠ {m}"); Console.ResetColor(); }

        private static void MostrarDespedida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════╗
║           ¡Gracias por usar el sistema!                  ║
║        Sistema de Gestión Universitaria v1.0             ║
╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔══════════════ MENÚ PRINCIPAL ══════════════╗");
            Console.ResetColor();
            Console.WriteLine("║  1. Gestionar Estudiantes                  ║");
            Console.WriteLine("║  2. Gestionar Profesores                   ║");
            Console.WriteLine("║  3. Gestionar Cursos                       ║");
            Console.WriteLine("║  4. Matricular Estudiante                  ║");
            Console.WriteLine("║  5. Registrar Calificaciones               ║");
            Console.WriteLine("║  6. Ver Reportes                           ║");
            Console.WriteLine("║  7. Análisis con Reflection                ║");
            Console.WriteLine("║  8. Demostrar Funcionalidades              ║");
            Console.WriteLine("║  9. Salir                                  ║");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("\nSeleccione una opción: ");
        }

        // ====== Estudiantes ======
        private void GestionarEstudiantes()
        {
            MarcoTitulo("GESTIÓN DE ESTUDIANTES", ConsoleColor.Green);
            Console.WriteLine("\n1. Agregar Estudiante\n2. Listar Estudiantes\n3. Buscar Estudiante\n4. Eliminar Estudiante\n5. Volver");
            Console.Write("\nOpción: ");
            switch (LeerOpcion())
            {
                case 1: AgregarEstudiante(); break;
                case 2: ListarEstudiantes(); break;
                case 3: BuscarEstudiante(); break;
                case 4: EliminarEstudiante(); break;
            }
        }

        private void AgregarEstudiante()
        {
            Console.WriteLine("\n--- Agregar Estudiante ---");
            try
            {
                Console.Write("ID: "); var id = Console.ReadLine();
                Console.Write("Nombre: "); var nombre = Console.ReadLine();
                Console.Write("Apellido: "); var apellido = Console.ReadLine();
                Console.Write("Fecha de Nacimiento (dd/MM/yyyy): ");
                var fecha = DateTime.ParseExact(Console.ReadLine()!, "dd/MM/yyyy", null);
                Console.Write("Carrera: "); var carrera = Console.ReadLine();
                Console.Write("Número de Matrícula: "); var numMat = Console.ReadLine();

                var est = new Estudiante(id!, nombre!, apellido!, fecha, carrera!, numMat!);
                repoEstudiantes.Agregar(est);
                MostrarExito("Estudiante agregado exitosamente");
            }
            catch (Exception ex) { MostrarError($"Error al agregar estudiante: {ex.Message}"); }
        }

        private void ListarEstudiantes()
        {
            Console.WriteLine("\n--- Lista de Estudiantes ---");
            var lst = repoEstudiantes.ObtenerTodos();
            if (lst.Count == 0) { MostrarWarn("No hay estudiantes registrados"); return; }
            lst.ForEach(e => Console.WriteLine($"\n{e}"));
            Console.WriteLine($"\nTotal: {lst.Count} estudiantes");
        }

        private void BuscarEstudiante()
        {
            Console.Write("\nIngrese ID del estudiante: "); var id = Console.ReadLine();
            var e = repoEstudiantes.BuscarPorId(id!);
            if (e != null) Console.WriteLine($"\n{e}");
            else MostrarWarn("Estudiante no encontrado");
        }

        private void EliminarEstudiante()
        {
            Console.Write("\nIngrese ID del estudiante a eliminar: "); var id = Console.ReadLine();
            if (repoEstudiantes.Eliminar(id!)) MostrarExito("Estudiante eliminado exitosamente");
            else MostrarWarn("Estudiante no encontrado");
        }

        // ====== Profesores ======
        private void GestionarProfesores()
        {
            MarcoTitulo("GESTIÓN DE PROFESORES", ConsoleColor.Green);
            Console.WriteLine("\n1. Agregar Profesor\n2. Listar Profesores\n3. Buscar Profesor\n4. Eliminar Profesor\n5. Volver");
            Console.Write("\nOpción: ");
            switch (LeerOpcion())
            {
                case 1: AgregarProfesor(); break;
                case 2: ListarProfesores(); break;
                case 3: BuscarProfesor(); break;
                case 4: EliminarProfesor(); break;
            }
        }

        private void AgregarProfesor()
        {
            Console.WriteLine("\n--- Agregar Profesor ---");
            try
            {
                Console.Write("ID: "); var id = Console.ReadLine();
                Console.Write("Nombre: "); var nombre = Console.ReadLine();
                Console.Write("Apellido: "); var apellido = Console.ReadLine();
                Console.Write("Fecha de Nacimiento (dd/MM/yyyy): ");
                var fecha = DateTime.ParseExact(Console.ReadLine()!, "dd/MM/yyyy", null);
                Console.Write("Departamento: "); var dep = Console.ReadLine();
                Console.WriteLine("Tipo de Contrato:\n1. Tiempo Completo\n2. Medio Tiempo\n3. Por Horas");
                Console.Write("Opción: "); var tipo = (TipoContrato)(LeerOpcion() - 1);
                Console.Write("Salario Base: "); var salario = decimal.Parse(Console.ReadLine()!);

                var prof = new Profesor(id!, nombre!, apellido!, fecha, dep!, tipo, salario);
                repoProfesores.Agregar(prof);
                MostrarExito("Profesor agregado exitosamente");
            }
            catch (Exception ex) { MostrarError($"Error al agregar profesor: {ex.Message}"); }
        }

        private void ListarProfesores()
        {
            Console.WriteLine("\n--- Lista de Profesores ---");
            var lst = repoProfesores.ObtenerTodos();
            if (lst.Count == 0) { MostrarWarn("No hay profesores registrados"); return; }
            foreach (var p in lst)
            {
                Console.WriteLine($"\n{p}");
                Console.WriteLine($"  Departamento: {p.Departamento}");
                Console.WriteLine($"  Contrato: {p.TipoContrato}");
                Console.WriteLine($"  Salario: ${p.SalarioBase:N2}");
            }
            Console.WriteLine($"\nTotal: {lst.Count} profesores");
        }

        private void BuscarProfesor()
        {
            Console.Write("\nIngrese ID del profesor: "); var id = Console.ReadLine();
            var p = repoProfesores.BuscarPorId(id!);
            if (p != null)
            {
                Console.WriteLine($"\n{p}");
                Console.WriteLine($"Departamento: {p.Departamento}");
                Console.WriteLine($"Contrato: {p.TipoContrato}");
                Console.WriteLine($"Salario: ${p.SalarioBase:N2}");
            }
            else MostrarWarn("Profesor no encontrado");
        }

        private void EliminarProfesor()
        {
            Console.Write("\nIngrese ID del profesor a eliminar: "); var id = Console.ReadLine();
            if (repoProfesores.Eliminar(id!)) MostrarExito("Profesor eliminado exitosamente");
            else MostrarWarn("Profesor no encontrado");
        }

        // ====== Cursos ======
        private void GestionarCursos()
        {
            MarcoTitulo("GESTIÓN DE CURSOS", ConsoleColor.Green);
            Console.WriteLine("\n1. Agregar Curso\n2. Listar Cursos\n3. Asignar Profesor a Curso\n4. Volver");
            Console.Write("\nOpción: ");
            switch (LeerOpcion())
            {
                case 1: AgregarCurso(); break;
                case 2: ListarCursos(); break;
                case 3: AsignarProfesor(); break;
            }
        }

        private void AgregarCurso()
        {
            Console.WriteLine("\n--- Agregar Curso ---");
            try
            {
                Console.Write("Código: "); var codigo = Console.ReadLine();
                Console.Write("Nombre: "); var nombre = Console.ReadLine();
                Console.Write("Créditos: "); var creditos = int.Parse(Console.ReadLine()!);

                var curso = new Curso(codigo!, nombre!, creditos);
                repoCursos.Agregar(curso);
                MostrarExito("Curso agregado exitosamente");
            }
            catch (Exception ex) { MostrarError($"Error al agregar curso: {ex.Message}"); }
        }

        private void ListarCursos()
        {
            Console.WriteLine("\n--- Lista de Cursos ---");
            var lst = repoCursos.ObtenerTodos();
            if (lst.Count == 0) { MostrarWarn("No hay cursos registrados"); return; }
            lst.ForEach(c => Console.WriteLine($"\n{c}"));
            Console.WriteLine($"\nTotal: {lst.Count} cursos");
        }

        private void AsignarProfesor()
        {
            Console.Write("\nIngrese código del curso: "); var cod = Console.ReadLine();
            var curso = repoCursos.BuscarPorId(cod!);
            if (curso == null) { MostrarWarn("Curso no encontrado"); return; }

            Console.Write("Ingrese ID del profesor: "); var idProf = Console.ReadLine();
            var prof = repoProfesores.BuscarPorId(idProf!);
            if (prof == null) { MostrarWarn("Profesor no encontrado"); return; }

            curso.ProfesorAsignado = prof;
            MostrarExito($"Profesor {prof.Nombre} {prof.Apellido} asignado al curso {curso.Nombre}");
        }

        // ====== Matrículas / Calificaciones ======
        private void MatricularEstudianteEnCurso()
        {
            MarcoTitulo("MATRICULAR ESTUDIANTE EN CURSO", ConsoleColor.White);
            try
            {
                Console.Write("ID del Estudiante: "); var idEst = Console.ReadLine();
                var est = repoEstudiantes.BuscarPorId(idEst!);
                if (est == null) { MostrarWarn("Estudiante no encontrado"); return; }

                Console.Write("Código del Curso: "); var cod = Console.ReadLine();
                var curso = repoCursos.BuscarPorId(cod!);
                if (curso == null) { MostrarWarn("Curso no encontrado"); return; }

                gestorMatriculas.MatricularEstudiante(est, curso);
                MostrarExito($"Estudiante {est.Nombre} matriculado en {curso.Nombre}");
            }
            catch (Exception ex) { MostrarError($"Error: {ex.Message}"); }
        }

        private void RegistrarCalificaciones()
        {
            MarcoTitulo("REGISTRAR CALIFICACIÓN", ConsoleColor.White);
            try
            {
                Console.Write("ID del Estudiante: "); var idEst = Console.ReadLine();
                Console.Write("Código del Curso: "); var cod = Console.ReadLine();
                Console.Write("Calificación (0-10): "); var cal = decimal.Parse(Console.ReadLine()!);

                gestorMatriculas.AgregarCalificacion(idEst!, cod!, cal);
                MostrarExito("Calificación registrada exitosamente");
            }
            catch (Exception ex) { MostrarError($"Error: {ex.Message}"); }
        }

        // ====== Reportes / Reflection / Demo ======
        private void VerReportes()
        {
            MarcoTitulo("REPORTES", ConsoleColor.Cyan);
            Console.WriteLine("\n1. Reporte de Estudiante\n2. Estudiantes por Curso\n3. Top 10 Estudiantes\n4. Estadísticas Generales\n5. Volver");
            Console.Write("\nOpción: ");
            switch (LeerOpcion())
            {
                case 1: ReporteEstudiante(); break;
                case 2: EstudiantesPorCurso(); break;
                case 3: TopEstudiantes(); break;
                case 4: EstadisticasGenerales(); break;
            }
        }

        private void ReporteEstudiante()
        {
            Console.Write("\nIngrese ID del estudiante: "); var id = Console.ReadLine();
            var rep = gestorMatriculas.GenerarReporteEstudiante(id!);
            Console.WriteLine(rep);
        }

        private void EstudiantesPorCurso()
        {
            Console.Write("\nIngrese código del curso: "); var cod = Console.ReadLine();
            var mats = gestorMatriculas.ObtenerEstudiantesPorCurso(cod!);
            if (mats.Count == 0) { MostrarWarn("No hay estudiantes matriculados en este curso"); return; }

            Console.WriteLine($"\nEstudiantes en el curso {cod}:");
            foreach (var m in mats)
                Console.WriteLine($"  - {m.Estudiante.Nombre} {m.Estudiante.Apellido} - Promedio: {m.ObtenerPromedio():F2}");
            Console.WriteLine($"\nTotal: {mats.Count} estudiantes");
        }

        private void TopEstudiantes()
        {
            Console.WriteLine("\n--- Top 10 Estudiantes ---");
            var top = gestorMatriculas.ObtenerTop10Estudiantes();
            int pos = 1;
            foreach (var e in top)
            {
                var mats = gestorMatriculas.ObtenerMatriculasPorEstudiante(e.Identificacion);
                var prom = mats.Where(m => m.Calificaciones.Count > 0).Average(m => m.ObtenerPromedio());
                Console.WriteLine($"{pos}. {e.Nombre} {e.Apellido} - Promedio: {prom:F2}");
                pos++;
            }
        }

        private void EstadisticasGenerales()
        {
            Console.WriteLine("\n--- Estadísticas Generales ---");
            Console.WriteLine($"\nTotal de Estudiantes: {repoEstudiantes.Contar()}");
            Console.WriteLine($"Total de Profesores: {repoProfesores.Contar()}");
            Console.WriteLine($"Total de Cursos: {repoCursos.Contar()}");
            Console.WriteLine($"Promedio General: {gestorMatriculas.ObtenerPromedioGeneral():F2}");

            var riesgo = gestorMatriculas.ObtenerEstudiantesEnRiesgo();
            Console.WriteLine($"Estudiantes en Riesgo: {riesgo.Count}");

            Console.WriteLine("\nEstadísticas por Carrera:");
            var stats = gestorMatriculas.ObtenerEstadisticasPorCarrera();
            foreach (var kv in stats)
            {
                dynamic v = kv.Value;
                Console.WriteLine($"\n  {kv.Key}:\n    Estudiantes: {v.Cantidad}\n    Promedio: {v.PromedioGeneral:F2}");
            }
        }

        private void AnalizarConReflection()
        {
            MarcoTitulo("ANÁLISIS CON REFLECTION", ConsoleColor.Magenta);
            var analizador = new AnalizadorReflection();


            Console.WriteLine("\n1. Analizar Estudiante\n2. Analizar Profesor\n3. Analizar Curso");
            Console.Write("\nOpción: ");
            Type? tipo = LeerOpcion() switch
            {
                1 => typeof(Estudiante),
                2 => typeof(Profesor),
                3 => typeof(Curso),
                _ => null
            };

            if (tipo != null)
            {
                analizador.MostrarPropiedades(tipo);
                analizador.MostrarMetodos(tipo);
            }
        }

        private void DemostrarFuncionalidades()
        {
            Console.Clear();
            generadorDatos.DemostrarFuncionalidades(repoEstudiantes, repoProfesores, repoCursos, gestorMatriculas);
        }
    }
}
