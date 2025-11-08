namespace Sistema_Universitario.Dominio.Interfaces
{
    
    /// Define un contrato para las clases que poseen una identificación única.
    /// Permite que las entidades sean administradas en repositorios genéricos.
   
    public interface IIdentificable
    {
        
        /// Identificador único de la entidad.
        
        string Identificacion { get; }
    }
}
