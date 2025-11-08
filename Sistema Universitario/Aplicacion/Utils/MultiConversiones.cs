using System;
using System.Collections.Generic;

namespace Sistema_Universitario.Aplicacion.Utils
{
    /// <summary>
    /// Clase que contiene métodos de conversión de tipos, 
    /// parsing de datos y demostración de boxing/unboxing.
    /// </summary>
    public class MultiConversiones
    {
        /// <summary>
        /// Convierte distintos tipos de datos a una representación textual amigable.
        /// </summary>
        /// <param name="dato">Cualquier objeto (valor o referencia).</param>
        /// <returns>Texto descriptivo según el tipo detectado.</returns>
        public string ConvertirDatos(object dato)
        {
            return dato switch
            {
                int i => $"Entero: {i}",
                double d => $"Decimal: {d:F2}",
                decimal dec => $"Decimal: {dec:F2}",
                string s => $"Texto: {s}",
                DateTime dt => $"Fecha: {dt:dd/MM/yyyy}",
                bool b => $"Booleano: {(b ? "Sí" : "No")}",
                _ => $"Tipo desconocido: {dato?.GetType().Name ?? "null"}"
            };
        }

        /// <summary>
        /// Intenta analizar un texto como calificación numérica (0-10).
        /// </summary>
        public bool ParsearCalificacion(string entrada, out decimal calificacion)
        {
            return decimal.TryParse(entrada, out calificacion) &&
                   calificacion >= 0 && calificacion <= 10;
        }

        /// <summary>
        /// Demuestra el funcionamiento del Boxing y Unboxing en C#.
        /// </summary>
        public void DemostrarBoxingUnboxing()
        {
            Console.WriteLine("\n=== Demostración de Boxing/Unboxing ===");

            // BOXING: convertir un tipo valor a tipo referencia
            int calificacionInt = 8;
            object calificacionBoxed = calificacionInt; // boxing
            Console.WriteLine($"Boxing: int {calificacionInt} → object");

            // UNBOXING: convertir tipo referencia a tipo valor
            int calificacionUnboxed = (int)calificacionBoxed; // unboxing
            Console.WriteLine($"Unboxing: object → int {calificacionUnboxed}");

            // Demostración con diferentes tipos
            Console.WriteLine("\nProcesando varios tipos de datos:");
            List<object> datos = new() { 7.5, 8, 9.0m, "10", true, DateTime.Now };

            foreach (var d in datos)
            {
                Console.WriteLine($"  {ConvertirDatos(d)}");
            }
        }
    }
}
