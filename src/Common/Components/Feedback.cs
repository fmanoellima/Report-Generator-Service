using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.Components
{
    /// <summary>
    /// Classe utilizada para padronizar os retornos de métodos e requisição ajax.
    /// </summary>
    [Serializable]
    public class Feedback
    {
        #region Enumeradores
        public enum TypeFeedback { Success, Error };
        #endregion

        #region Atributos
        [XmlIgnore]
        private List<string> _messages = new List<string>();
        #endregion

        #region Propriedades
        /// <summary>
        /// Propriedade pública que define o status do feedback.
        /// </summary>
        
        public TypeFeedback Status
        {
            get;
            set;
        }

        /// <summary>
        /// Propriedade pública que define o código identificador do projeto.
        /// </summary>
        
        public int IdProjeto
        {
            get;
            set;
        }

        /// <summary>
        /// Propriedade pública que define a mensagem de retorno do feedback.
        /// </summary>
        public List<String> Message
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
            }
        }

        /// <summary>
        /// Propriedade pública que define o objeto de retorno output.
        /// </summary>
        public Object Output
        {
            get;
            set;
        }
        #endregion

        #region Construtor
        /// <summary>
        /// Método construtor padrão.
        /// </summary>
        public Feedback()
        { }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>        
        public Feedback(TypeFeedback _type)
        {
            Status = _type;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>        
        /// <param name="_idprojeto">Código identificador do projeto.</param>
        public Feedback(TypeFeedback _type, int _idprojeto)
        {
            Status = _type;
            IdProjeto = _idprojeto;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_message">Parâmetro que define a mensagem de retorno.</param>
        public Feedback(TypeFeedback _type, String _message)
        {
            Status = _type;
            Message.Add(_message);
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_idprojeto">Código identificador do projeto.</param>
        /// <param name="_message">Parâmetro que define a mensagem de retorno.</param>
        public Feedback(TypeFeedback _type, int _idprojeto, String _message)
        {
            Status = _type;
            IdProjeto = _idprojeto;
            Message.Add(_message);
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_list">Parâmetro que define a lista de mensagens de retorno.</param>
        public Feedback(TypeFeedback _type, List<String> _list)
        {
            Status = _type;
            Message = _list;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_idprojeto">Código identificador do projeto.</param>
        /// <param name="_list">Parâmetro que define a lista de mensagens de retorno.</param>
        public Feedback(TypeFeedback _type, int _idprojeto, List<String> _list)
        {
            Status = _type;
            IdProjeto = _idprojeto;
            Message = _list;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>        
        /// <param name="_output">Parâmetro que define o objeto do output do feedback.</param>
        public Feedback(TypeFeedback _type, Object _output)
        {
            Status = _type;
            Output = _output;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>  
        /// <param name="_idprojeto">Código identificador do projeto.</param>
        /// <param name="_output">Parâmetro que define o objeto do output do feedback.</param>
        public Feedback(TypeFeedback _type, int _idprojeto, Object _output)
        {
            Status = _type;
            IdProjeto = _idprojeto;
            Output = _output;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_message">Parâmetro que define a mensagem de retorno.</param>
        /// <param name="_output">Parâmetro que define o objeto do output do feedback.</param>
        public Feedback(TypeFeedback _type, String _message, Object _output)
        {
            Status = _type;
            Output = _output;
            Message.Add(_message);
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_idprojeto">Código identificador do projeto.</param>
        /// <param name="_message">Parâmetro que define a mensagem de retorno.</param>
        /// <param name="_output">Parâmetro que define o objeto do output do feedback.</param>
        public Feedback(TypeFeedback _type, int _idprojeto, String _message, Object _output)
        {
            Status = _type;
            IdProjeto = _idprojeto;
            Output = _output;
            Message.Add(_message);
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_list">Parâmetro que define a lista de mensagens de retorno.</param>
        /// <param name="_output">Parâmetro que define o objeto do output do feedback.</param>
        public Feedback(TypeFeedback _type, List<String> _list, Object _output)
        {
            Status = _type;
            Message = _list;
            Output = _output;
        }

        /// <summary>
        /// Sobrecarga de construtor da classe feedback.
        /// </summary>
        /// <param name="_type">Enumerador que define o tipo do feedback.</param>
        /// <param name="_idprojeto">Código identificador do projeto.</param>
        /// <param name="_list">Parâmetro que define a lista de mensagens de retorno.</param>
        /// <param name="_output">Parâmetro que define o objeto do output do feedback.</param>
        public Feedback(TypeFeedback _type, int _idprojeto, List<String> _list, Object _output)
        {
            Status = _type;
            IdProjeto = _idprojeto;
            Message = _list;
            Output = _output;
        }
        #endregion
    }
}
