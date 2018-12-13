using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposEquipamentoVeiculo
    {
        public ObservableCollection<TipoEquipamentoVeiculo> TiposEquipamentoVeiculo = new ObservableCollection<TipoEquipamentoVeiculo>();
        public class TipoEquipamentoVeiculo
        {
            private int _TipoEquipamentoViculoID;
            private string _Descricao;
            public int TipoEquipamentoViculoID
            {
                get { return _TipoEquipamentoViculoID; }

                set { _TipoEquipamentoViculoID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}