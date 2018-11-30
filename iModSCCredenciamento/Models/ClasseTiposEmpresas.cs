using System.Collections.ObjectModel;

public class ClasseTiposEmpresas
{

    public ObservableCollection<TipoEmpresa> TiposEmpresas = new ObservableCollection<TipoEmpresa>();
    public class TipoEmpresa
    {
        private int _TipoEmpresaID;
        private string _TipoEmpresaDesc;
        public int TipoEmpresaID
        {
            get { return _TipoEmpresaID; }

            set { _TipoEmpresaID = value; }
        }
        public string TipoEmpresaDesc
        {
            get { return _TipoEmpresaDesc; }

            set { _TipoEmpresaDesc = value; }
        }
    }
}