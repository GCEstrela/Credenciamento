using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposCobrancas
    {
        public ObservableCollection<TipoCobranca> TiposCobrancas = new ObservableCollection<TipoCobranca>();

        public class TipoCobranca
        {
            private int _TipoCobrancaID;
            private string _Descricao;
            public int TipoCobrancaID
            {
                get { return _TipoCobrancaID; }

                set { _TipoCobrancaID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}
