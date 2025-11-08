using System;
using System.Linq;
using System.Reflection;

namespace SistemaUniversitario.Estructura
{
    /// <summary>
    /// Clase que utiliza Reflection para analizar tipos, crear instancias
    /// e invocar métodos de manera dinámica en tiempo de ejecución.
    /// </summary>
    public class AnalizadorReflection
    {
        /// <summary>
        /// Muestra en consola las propiedades públicas de un tipo.
        /// </summary>
        public void MostrarPropiedades(Type tipo)
        {
            Console.WriteLine($"\n=== Propiedades de {tipo.Name} ===");
            var propiedades = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in propiedades)
                Console.WriteLine($"  - {prop.Name} ({prop.PropertyType.Name})");
        }

        /// <summary>
        /// Muestra los métodos públicos declarados en un tipo.
        /// </summary>
        public void MostrarMetodos(Type tipo)
        {
            Console.WriteLine($"\n=== Métodos de {tipo.Name} ===");
            var metodos = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var metodo in metodos)
            {
                var parametros = string.Join(", ", metodo.GetParameters()
                    .Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"  - {metodo.ReturnType.Name} {metodo.Name}({parametros})");
            }
        }

        /// <summary>
        /// Crea dinámicamente una instancia de un tipo con parámetros opcionales.
        /// </summary>
        public object? CrearInstanciaDinamica(Type tipo, params object[] parametros)
        {
            try
            {
                return Activator.CreateInstance(tipo, parametros);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ Error al crear instancia: {ex.Message}");
                Console.ResetColor();
                return null;
            }
        }

        /// <summary>
        /// Invoca un método de una instancia usando su nombre y parámetros.
        /// </summary>
        public object? InvocarMetodo(object instancia, string nombreMetodo, params object[] parametros)
        {
            try
            {
                var tipo = instancia.GetType();
                var metodo = tipo.GetMethod(nombreMetodo);

                if (metodo == null)
                    throw new InvalidOperationException($"No se encontró el método '{nombreMetodo}' en la clase {tipo.Name}");

                return metodo.Invoke(instancia, parametros);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ Error al invocar método: {ex.Message}");
                Console.ResetColor();
                return null;
            }
        }
    }
}
