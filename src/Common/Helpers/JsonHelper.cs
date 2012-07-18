using Newtonsoft.Json;

namespace Common.Helpers
{
    /// <summary>
    /// Classe que contém métodos e propriedades relacionadas a JSON.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Método estático público que converto um objeto T para um string JSON.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto anônimo.</typeparam>
        /// <param name="_object">Objeto que contem os dados.</param>
        /// <returns>Retorna a string JSON com o dados serializados.</returns>
        public static string ConvertToJSON<T>(this T _object)
        {
            return JsonConvert.SerializeObject(_object);
        }

        /// <summary>
        /// Método estático que converte um JSON para objeto T.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto anônimo.</typeparam>
        /// <param name="_json">String JSON com os dados que serão convertidos.</param>
        /// <returns>Retorno o objeto T com seus dados deserializados.</returns>
        public static T ConvertToObject<T>(this string _json)
        {
            T _object;
            _object = (T)JsonConvert.DeserializeObject(_json, typeof(T));

            return _object;
        }
    }
}