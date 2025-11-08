namespace Sistema_Universitario.Dominio.Interfaces
{
    /// <summary>
    /// Define un contrato para las clases que poseen una identificación única.
    /// Permite que las entidades sean administradas en repositorios genéricos.
    /// </summary>
    public interface IIdentificable
    {
        /// <summary>
        /// Identificador único de la entidad.
        /// </summary>
        string Identificacion { get; }
    }
}
