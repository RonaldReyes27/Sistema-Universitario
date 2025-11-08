using System.Collections.Generic;
using System.Reflection;

namespace SistemaUniversitario.Aplicacion.Validacion
{
    public class Validador
    {
        public List<string> ValidarObjeto(object obj)
        {
            var errores = new List<string>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var valor = prop.GetValue(obj);

                // Requerido
                if (prop.GetCustomAttribute<RequeridoAttribute>() != null)
                {
                    if (valor == null || (valor is string s && string.IsNullOrWhiteSpace(s)))
                        errores.Add($"La propiedad {prop.Name} es requerida");
                }

                // Rango (usa double internamente)
                var rango = prop.GetCustomAttribute<ValidacionRangoAttribute>();
                if (rango != null && valor != null)
                {
                    double? num = valor switch
                    {
                        int i => i,
                        long l => l,
                        float f => f,
                        double d => d,
                        decimal m => (double)m,
                        _ => null
                    };

                    if (num.HasValue && (num.Value < rango.Minimo || num.Value > rango.Maximo))
                        errores.Add($"La propiedad {prop.Name} debe estar entre {rango.Minimo} y {rango.Maximo}");
                }

                // Formato (validación simple por longitud)
                var formato = prop.GetCustomAttribute<FormatoAttribute>();
                if (formato != null && valor is string str && !string.IsNullOrEmpty(str))
                {
                    if (str.Length != formato.Patron.Length)
                        errores.Add($"La propiedad {prop.Name} no cumple con el formato: {formato.Patron}");
                }
            }
            return errores;
        }
    }
}
