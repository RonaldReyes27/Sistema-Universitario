using System;
using System.Collections.Generic;
using System.Linq;
using Sistema_Universitario.Dominio.Interfaces;

namespace Sistema_Universitario.Aplicacion
{
    /// <summary>
    /// Repositorio genérico para gestionar entidades que implementen IIdentificable.
    /// Permite agregar, eliminar, buscar y listar elementos de manera centralizada.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que implementa IIdentificable.</typeparam>
    public class Repositorio<T> where T : IIdentificable
    {
        private readonly Dictionary<string, T> _items = new();

        /// <summary>
        /// Agrega un nuevo elemento al repositorio.
        /// </summary>
        public void Agregar(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_items.ContainsKey(item.Identificacion))
                throw new InvalidOperationException($"Ya existe un elemento con la identificación {item.Identificacion}");

            _items.Add(item.Identificacion, item);
        }

        /// <summary>
        /// Elimina un elemento por su identificación.
        /// </summary>
        public bool Eliminar(string id) => _items.Remove(id);

        /// <summary>
        /// Busca un elemento por su identificación.
        /// </summary>
        public T? BuscarPorId(string id)
        {
            _items.TryGetValue(id, out T? item);
            return item;
        }

        /// <summary>
        /// Obtiene todos los elementos almacenados.
        /// </summary>
        public List<T> ObtenerTodos() => _items.Values.ToList();

        /// <summary>
        /// Busca elementos que cumplan una condición.
        /// </summary>
        public List<T> Buscar(Func<T, bool> predicado) => _items.Values.Where(predicado).ToList();

        /// <summary>
        /// Devuelve la cantidad total de elementos almacenados.
        /// </summary>
        public int Contar() => _items.Count;
    }
}
