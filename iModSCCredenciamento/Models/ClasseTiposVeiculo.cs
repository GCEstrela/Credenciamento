using System.Collections.ObjectModel;

public class ClasseTiposVeiculo
{


    public ObservableCollection<TipoVeiculo> TiposVeiculo = new ObservableCollection<TipoVeiculo>();
    public class TipoVeiculo
    {
        private int _TipoVeiculoID;
        private string _TipoVeiculoCatHab;
        private string _TipoVeiculoTipo;
        public int TipoVeiculoID
        {
            get { return _TipoVeiculoID; }

            set { _TipoVeiculoID = value; }
        }
        public string TipoVeiculoCatHab
        {
            get { return _TipoVeiculoCatHab; }

            set { _TipoVeiculoCatHab = value; }
        }
        public string TipoVeiculoTipo
        {
            get { return _TipoVeiculoTipo; }

            set { _TipoVeiculoTipo = value; }
        }
    }
}