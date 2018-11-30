using System.Collections.ObjectModel;

public class ClasseAreasAcessos
{

    public ObservableCollection<AreaAcesso> AreasAcessos = new ObservableCollection<AreaAcesso>();

    public class AreaAcesso
    {
        private int _AreaAcessoID;
        private string _Descricao;
        private string _Identificacao;

        public int AreaAcessoID
        {
            get { return _AreaAcessoID; }

            set { _AreaAcessoID = value; }
        }
        public string Descricao
        {
            get { return _Descricao; }

            set { _Descricao = value; }
        }
        public string Identificacao
        {
            get { return _Identificacao; }

            set { _Identificacao = value; }
        }
    }

}