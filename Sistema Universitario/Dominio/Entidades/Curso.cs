using Sistema_Universitario.Dominio.Interfaces;
using SistemaUniversitario.Aplicacion.Validacion;
using System;
using SistemaUniversitario.Dominio.Entidades;
using SistemaUniversitario.Dominio.Enums;


namespace SistemaUniversitario.Dominio.Entidades
{

    public class Curso : IIdentificable
    {
        // Campos privados
        private string codigo = "";
        private string nombre = "";

        //Propiedad: Código del curso
        // Obligatoria, no puede estar vacía.
        [Requerido]
        public string Codigo
        {
            get => codigo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException("El código no puede estar vacío");
                codigo = value;
            }
        }

        // Implementación de la interfaz IIdentificable
        // Retorna el código como identificador único.
        string IIdentificable.Identificacion => Codigo;



        // Propiedad: Nombre del curso
        // También es obligatoria.
        [Requerido]
        public string Nombre
        {
            get => nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException("El nombre no puede estar vacío");
                nombre = value;
            }
        }
        // Propiedad: Créditos del curso
        // Debe estar en el rango de 1 a 6.
        [ValidacionRango(1, 6)]
        public int Creditos { get; set; }

        // Profesor asignado al curso (opcional)
        public Profesor? ProfesorAsignado { get; set; }

        // Constructor por defecto
        public Curso() { }

        // Constructor completo
        // Permite crear el curso con todos los datos necesarios.
        public Curso(string codigo, string nombre, int creditos, Profesor? profesor = null)
        {
            Codigo = codigo;
            Nombre = nombre;
            Creditos = creditos;
            ProfesorAsignado = profesor;
        }

        // Representación textual del curso
        // Devuelve una descripción legible del curso con su profesor.
        public override string ToString()
        {
            var prof = ProfesorAsignado != null
                ? $"{ProfesorAsignado.Nombre} {ProfesorAsignado.Apellido}"
                : "Sin asignar";
            return $"{Codigo} - {Nombre} ({Creditos} créditos) - Profesor: {prof}";
        }
    }
}
