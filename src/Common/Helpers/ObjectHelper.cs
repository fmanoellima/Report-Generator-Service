using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    /// <summary>
    /// Classe que contém métodos e propriedades relacionadas a objetos.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Método público que transporta os dados da lista de origem para lista de destino.
        /// Este método é útil porque, ao transportar dados entre lista de tipos distintos ocorre 
        /// erros de conversão, utilizando essa método esse problema é resolvido.
        /// </summary>
        /// <typeparam name="FROM_TYPE">Tipo de origem.</typeparam>
        /// <typeparam name="TO_TYPE">Tipo de destino.</typeparam>
        /// <param name="_listFrom">Objeto lista de origem.</param>
        /// <param name="_listTo">Objeto lista de destino</param>
        /// <returns>Retorna a lista de destino preenchida com os dados da lista de origem.</returns>
        public static List<TO_TYPE> AddRange<FROM_TYPE, TO_TYPE>(List<FROM_TYPE> _listFrom, List<TO_TYPE> _listTo) where FROM_TYPE : TO_TYPE
        {
            // percorre os dados da lista de origem  
            _listTo.AddRange(_listFrom.Select(item => (TO_TYPE) item));

            // retorna a lista de destino devidamente preenchida
            return _listTo;
        }
    }
}