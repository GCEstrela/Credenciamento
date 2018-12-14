using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposCombustiveis
    {
        public ObservableCollection<TipoCombustivel> TiposCombustiveis = new ObservableCollection<TipoCombustivel>();
        public class TipoCombustivel
        {
            private int _TipoCombustivelID;
            private string _Descricao;
            public int TipoCombustivelID
            {
                get { return _TipoCombustivelID; }

                set { _TipoCombustivelID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}
