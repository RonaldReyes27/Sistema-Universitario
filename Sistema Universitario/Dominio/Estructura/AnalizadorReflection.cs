using System;
using System.Linq;
using System.Reflection;
using SistemaUniversitario.Estructura;

namespace SistemaUniversitario.Estructura
{
    
    /// Clase que utiliza Reflection para analizar tipos, crear instancias
    /// e invocar métodos de manera dinámica en tiempo de ejecución.
    
    public class AnalizadorReflection
    {
       
        /// Muestra en consola las propiedades públicas de un tipo.
       
        public void MostrarPropiedades(Type tipo)
        {
            Console.WriteLine($"\n=== Propiedades de {tipo.Name} ===");
            var propiedades = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in propiedades)
                Console.WriteLine($"  - {prop.Name} ({prop.PropertyType.Name})");
        }

        /// Muestra los métodos públicos declarados en un tipo.
        
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

        
        /// Crea dinámicamente una instancia de un tipo con parámetros opcionales.
       
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

        
        /// Invoca un método de una instancia usando su nombre y parámetros.
        
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
