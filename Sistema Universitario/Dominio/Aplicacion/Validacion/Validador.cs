using System;
using System.Collections.Generic;
using System.Reflection;

namespace SistemaUniversitario.Aplicacion.Validacion
{
    /// <summary>
    /// Clase encargada de validar objetos que usan los atributos personalizados
    /// [Requerido], [ValidacionRango] y [Formato].
    /// </summary>
    public class Validador
    {
        /// <summary>
        /// Valida todas las propiedades de un objeto según los atributos decoradores.
        /// </summary>
        /// <param name="obj">Objeto a validar.</param>
        /// <returns>Lista de errores encontrados (vacía si es válido).</returns>
        public List<string> ValidarObjeto(object obj)
        {
            var errores = new List<string>();
            var tipo = obj.GetType();
            var propiedades = tipo.GetProperties();

            foreach (var prop in propiedades)
            {
                var valor = prop.GetValue(obj);

                // ===== Validación de [Requerido] =====
                var requerido = prop.GetCustomAttribute<RequeridoAttribute>();
                if (requerido != null)
                {
                    if (valor == null || (valor is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        errores.Add($"La propiedad {prop.Name} es requerida.");
                    }
                }

                // ===== Validación de [ValidacionRango] =====
                var rango = prop.GetCustomAttribute<ValidacionRangoAttribute>();
                if (rango != null && valor != null)
                {
                    if (valor is decimal dec)
                    {
                        if (dec < rango.Minimo || dec > rango.Maximo)
                            errores.Add($"La propiedad {prop.Name} debe estar entre {rango.Minimo} y {rango.Maximo}.");
                    }
                    else if (valor is int entero)
                    {
                        if (entero < rango.Minimo || entero > rango.Maximo)
                            errores.Add($"La propiedad {prop.Name} debe estar entre {rango.Minimo} y {rango.Maximo}.");
                    }
                }

                // ===== Validación de [Formato] =====
                var formato = prop.GetCustomAttribute<FormatoAttribute>();
                if (formato != null && valor is string texto && !string.IsNullOrEmpty(texto))
                {
                    // Validación simple de longitud (puedes ampliar con Regex)
                    if (texto.Length != formato.Patron.Length)
                    {
                        errores.Add($"La propiedad {prop.Name} no cumple el formato esperado: {formato.Patron}");
                    }
                }
            }

            return errores;
        }
    }
}
