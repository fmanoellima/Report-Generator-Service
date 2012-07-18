using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Common.Helpers;


namespace Common.Components
{
    /// <summary>
    /// Classe utilizada para serializar as informações enviadas ao jQGrid.
    /// </summary>
    /// <typeparam name="T">Anonymous type</typeparam>
    public class jQGrid<T>
    {
        #region Atributos
        /// <summary>
        /// Atributo T que armazena a lista de objetos.
        /// </summary>
        private List<T> _rows;
        #endregion

        #region Propriedades privados
        /// <summary>
        /// Propriedade privada que define o range inicial de exibição dos registros na paginação.
        /// </summary>
        private int start
        {
            get
            {
                return (linhas * (page - 1));
            }
        }

        /// <summary>
        /// Propriedade privada que define o range final de exibição dos registros na paginação.
        /// </summary>
        private int limit
        {
            get
            {
                return ((records - linhas * (page - 1)) < linhas ? (records - linhas * (page - 1)) : linhas);
            }
        }

        /// <summary>
        /// Propriedade privada que identifica o index da coluna de ordenação.
        /// </summary>
        private string sidx
        {
            get
            {
                return HttpContext.Current.Request["sidx"];
            }
        }

        /// <summary>
        /// Propriedade privada que identifica o tipo de ordenação (asc; desc).
        /// </summary>
        private string sord
        {
            get
            {
                return HttpContext.Current.Request["sord"];
            }
        }
        #endregion

        #region Propriedades públicas
        /// <summary>
        /// Propriedade pública que define a quantidade de linhas por página.
        /// </summary>
        public int linhas
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["rows"]))
                {
                    return (int.Parse(HttpContext.Current.Request["rows"]) == 0 ? 10 : int.Parse(HttpContext.Current.Request["rows"]));
                }

                return 1;
            }
        }

        /// <summary>
        /// Propriedade pública que define a linhas de exibição.
        /// </summary>
        public int records
        {
            get;
            set;
        }

        /// <summary>
        /// Propriedade pública que define a quantidade de páginas totais.
        /// </summary>
        public int total
        {
            get
            {
                return (linhas * (records / linhas) >= records ? (records / linhas) : ((records / linhas) + 1));
            }
        }

        /// <summary>
        /// Propriedade pública que recupera o índice da página que será visualizada.
        /// </summary>
        public int currentPage
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["page"]))
                {
                    return (int.Parse(HttpContext.Current.Request["page"]) == 0 ? 1 : int.Parse(HttpContext.Current.Request["page"]));
                }

                return 1;
            }
        }

        /// <summary>
        /// Propriedade pública que define a página corrente.
        /// </summary>
        public int page
        {
            get
            {
                return (currentPage > total ? total : currentPage);
            }
        }

        /// <summary>
        /// Propriedade pública mantém os dados das linhas.
        /// </summary>
        public List<T> rows
        {
            get
            {
                if (sidx != String.Empty && _rows != null)
                {
                    System.Reflection.PropertyInfo _property = _rows[0].GetType().GetProperty(sidx);

                    if (_property != null)
                    {
                        _rows = (sord == "asc" ?
                            (from _data in _rows orderby _property.GetValue(_data, null) select _data).ToList() :
                            (from _data in _rows orderby _property.GetValue(_data, null) descending select _data).ToList()
                        );
                    }
                }

                return _rows.Skip(start).Take(limit).ToList<T>();
            }
            set
            {
                _rows = value;
            }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Método público que converte o objeto em string JSON.
        /// </summary>
        /// <returns></returns>
        public string toJSON()
        {
            return JsonHelper.ConvertToJSON(this);
        }
        #endregion
    }
}
