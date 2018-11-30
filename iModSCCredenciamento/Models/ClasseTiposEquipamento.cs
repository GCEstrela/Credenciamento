using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposEquipamento
    {

        public ObservableCollection<TipoEquipamento> TiposEquipamentos = new ObservableCollection<TipoEquipamento>();
        public class TipoEquipamento
        {
            private int _TipoEquipamentoID;
            private string _Descricao;
            public int TipoEquipamentoID
            {
                get { return _TipoEquipamentoID; }

                set { _TipoEquipamentoID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}
