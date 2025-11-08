using System;

namespace SistemaUniversitario.Aplicacion.Validacion
{
    /// <summary>
    /// Atributo que valida que una propiedad numérica esté dentro de un rango específico.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidacionRangoAttribute : Attribute
    {
        public decimal Minimo { get; set; }
        public decimal Maximo { get; set; }

        public ValidacionRangoAttribute(decimal minimo, decimal maximo)
        {
            Minimo = minimo;
            Maximo = maximo;
        }
    }

    /// <summary>
    /// Atributo que indica que una propiedad es requerida (no puede ser nula o vacía).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequeridoAttribute : Attribute
    {
    }

    /// <summary>
    /// Atributo que define un formato esperado para cadenas de texto.
    /// Ejemplo: [Formato("XXX-XXXXX")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatoAttribute : Attribute
    {
        public string Patron { get; set; }

        public FormatoAttribute(string patron)
        {
            Patron = patron;
        }
    }
}
