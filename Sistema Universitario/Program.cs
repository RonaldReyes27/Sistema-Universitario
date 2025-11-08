using System;
using System.Text;
using SistemaUniversitario.Presentation;

namespace SistemaUniversitario
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Soporte de caracteres (acentos, líneas, etc.)
                Console.OutputEncoding = Encoding.UTF8;

                // Punto de entrada a la app de consola
                var sistema = new SistemaUniversitarioApp();
                sistema.Ejecutar();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ Error fatal: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para salir...");
                Console.ReadKey();
            }
        }
    }
}
