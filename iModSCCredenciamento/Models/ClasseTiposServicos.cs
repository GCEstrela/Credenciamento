using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposServicos
    {
        public ObservableCollection<TipoServico> TiposServicos = new ObservableCollection<TipoServico>();
        public class TipoServico
        {
            private int _TipoServicoID;
            private string _Descricao;
            public int TipoServicoID
            {
                get { return _TipoServicoID; }

                set { _TipoServicoID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}