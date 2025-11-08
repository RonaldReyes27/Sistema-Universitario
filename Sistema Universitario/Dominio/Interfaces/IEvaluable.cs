namespace Sistema_Universitario.Dominio.Interfaces
{
    
    /// Define el comportamiento que deben tener las entidades evaluables,
    /// como cursos o matrículas que pueden registrar calificaciones.
    
    public interface IEvaluable
    {
       
        /// Agrega una calificación a la entidad.
        
        /// <param name="calificacion">Valor decimal entre 0 y 10.</param>
        void AgregarCalificacion(decimal calificacion);

        
        /// Calcula y devuelve el promedio de las calificaciones registradas.
        
        /// <returns>Promedio en formato decimal.</returns>
        decimal ObtenerPromedio();

        
        /// Determina si la entidad ha alcanzado un estado aprobado.
        
        /// <returns>True si el promedio es mayor o igual a 7.0.</returns>
        bool HaAprobado();
    }
}
