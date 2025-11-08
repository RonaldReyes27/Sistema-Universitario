namespace Sistema_Universitario.Dominio.Interfaces
{
    /// <summary>
    /// Define el comportamiento que deben tener las entidades evaluables,
    /// como cursos o matrículas que pueden registrar calificaciones.
    /// </summary>
    public interface IEvaluable
    {
        /// <summary>
        /// Agrega una calificación a la entidad.
        /// </summary>
        /// <param name="calificacion">Valor decimal entre 0 y 10.</param>
        void AgregarCalificacion(decimal calificacion);

        /// <summary>
        /// Calcula y devuelve el promedio de las calificaciones registradas.
        /// </summary>
        /// <returns>Promedio en formato decimal.</returns>
        decimal ObtenerPromedio();

        /// <summary>
        /// Determina si la entidad ha alcanzado un estado aprobado.
        /// </summary>
        /// <returns>True si el promedio es mayor o igual a 7.0.</returns>
        bool HaAprobado();
    }
}
