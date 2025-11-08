using System;
using System.Collections.Generic;
using System.Linq;
using Sistema_Universitario.Dominio.Interfaces;

namespace Sistema_Universitario.Aplicacion
{
    
    /// Repositorio genérico para gestionar entidades que implementen IIdentificable.
    /// Permite agregar, eliminar, buscar y listar elementos de manera centralizada.
    
    /// <typeparam name="T">Tipo de entidad que implementa IIdentificable.</typeparam>
    public class Repositorio<T> where T : IIdentificable
    {
        private readonly Dictionary<string, T> _items = new();

        
        /// Agrega un nuevo elemento al repositorio.
       
        public void Agregar(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_items.ContainsKey(item.Identificacion))
                throw new InvalidOperationException($"Ya existe un elemento con la identificación {item.Identificacion}");

            _items.Add(item.Identificacion, item);
        }

        
        /// Elimina un elemento por su identificación.
        
        public bool Eliminar(string id) => _items.Remove(id);

        
        /// Busca un elemento por su identificación.
        
        public T? BuscarPorId(string id)
        {
            _items.TryGetValue(id, out T? item);
            return item;
        }

        
        /// Obtiene todos los elementos almacenados.
        
        public List<T> ObtenerTodos() => _items.Values.ToList();

        
        /// Busca elementos que cumplan una condición.
        
        public List<T> Buscar(Func<T, bool> predicado) => _items.Values.Where(predicado).ToList();

        
        /// Devuelve la cantidad total de elementos almacenados.
        
        public int Contar() => _items.Count;
    }
}
