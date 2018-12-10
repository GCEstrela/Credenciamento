using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
   public class ClasseTiposAcessos
    {
        public ObservableCollection<TipoAcesso> TiposAcessos = new ObservableCollection<TipoAcesso>();

        public class TipoAcesso
        {
            private int _TipoAcessoID;
            private string _Descricao;
            public int TipoAcessoID
            {
                get { return _TipoAcessoID; }

                set { _TipoAcessoID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}
