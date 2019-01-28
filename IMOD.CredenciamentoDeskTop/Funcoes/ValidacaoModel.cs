// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#endregion

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class ValidacaoModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private  ConcurrentDictionary<string, List<string>> _errors = new ConcurrentDictionary<string, List<string>>();
        private readonly object _lock = new object();

        #region  Propriedades

        public bool HasErrors
        {
            get { return _errors.Any (kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public List<string> Errors
        {
            get
            {
                var erros = new List<string>();
                erros.Clear();
                foreach (var item in _errors.Values)
                     item.ForEach(n=>erros.Add (n)); 
                 
                return erros;
            }
            
        }

        #endregion

        #region  Metodos

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler (this, new PropertyChangedEventArgs (propertyName));
            ValidateAsync();
        }

        public Task ValidateAsync()
        {
            return Task.Run (() => Validate());
        }

        /// <summary>
        /// Validação
        /// </summary>
        /// <param name="func"></param>
        /// <param name="key">Nome da propriedade da entidade</param>
        /// <param name="msg">Mensagem caso nçao passe na validaçao</param>
        public void Validate(Func<bool> func,string key,string msg)
        {
            Validate();
            var result = func.Invoke();
            if (!result) return;
            var n = new List<string>();
            n.Add(msg);
            _errors.TryAdd(key, n);
            OnErrorsChanged(key);

        }
        /// <summary>
        /// Set mensagem de erro
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        public void SetMessageErro(string key, string msg)
        {
            var n = new List<string>();
            n.Add(msg);
            _errors.TryAdd(key, n);
            OnErrorsChanged(key);
        }

        public void Validate()
        {
            lock (_lock)
             {
                var validationContext = new ValidationContext (this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject (this, validationContext, validationResults, true);

                foreach (var kv in _errors.ToList())
                {
                    if (validationResults.All (r => r.MemberNames.All (m => m != kv.Key)))
                    {
                        List<string> outLi;
                        _errors.TryRemove (kv.Key, out outLi);
                        OnErrorsChanged (kv.Key);
                    }
                }

                var q = from r in validationResults
                    from m in r.MemberNames
                    group r by m
                    into g
                    select g;

                foreach (var prop in q)
                {
                    var messages = prop.Select (r => r.ErrorMessage).ToList();

                    if (_errors.ContainsKey (prop.Key))
                    {
                        List<string> outLi;
                        _errors.TryRemove (prop.Key, out outLi);
                    }
                    _errors.TryAdd (prop.Key, messages);
                    OnErrorsChanged (prop.Key);
                }
             }
        }

         
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
                handler (this, new DataErrorsChangedEventArgs (propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            _errors.TryGetValue (propertyName, out errorsForName);
            return errorsForName;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }

        #endregion
    }
}