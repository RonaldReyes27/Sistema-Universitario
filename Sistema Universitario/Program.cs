using System;
using System.Text;
using SistemaUniversitario.Presentacion;   // <-- en español: Presentacion

namespace SistemaUniversitario
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                // Clase que vive en el namespace SistemaUniversitario.Presentacion
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
