using System;

namespace SistemaUniversitario.Aplicacion.Validacion
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidacionRangoAttribute : Attribute
    {
        // OJO: en atributos no se acepta decimal -> usa double
        public double Minimo { get; }
        public double Maximo { get; }

        public ValidacionRangoAttribute(double minimo, double maximo)
        {
            Minimo = minimo;
            Maximo = maximo;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequeridoAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class FormatoAttribute : Attribute
    {
        public string Patron { get; }
        public FormatoAttribute(string patron) { Patron = patron; }
    }
}
