using System;

namespace iModSCCredenciamento.Models
{
    public class Colaborador
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private int _colaboradorId;
        public int ColaboradorId
        {
            get { return _colaboradorId; }
            set { _colaboradorId = value; }
        }
        private string _credencialGuid;
        public string CredencialGuidId
        {
            get { return _credencialGuid; }
            set { _credencialGuid = value; }
        }

        private DateTime _dataValidade;
        public DateTime DataValidade
        {
            get { return _dataValidade; }
            set { _dataValidade = value; }
        }

        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }


    }
}
