using System.Collections.ObjectModel;

public class ClasseTiposAtividades
{
    
    public ObservableCollection<TipoAtividade> TiposAtividades = new ObservableCollection<TipoAtividade>();
    public class TipoAtividade
    {
        private int _TipoAtividadeID;
        private string _Descricao;
        public int TipoAtividadeID
        {
            get { return _TipoAtividadeID; }

            set { _TipoAtividadeID = value; }
        }
        public string Descricao
        {
            get { return _Descricao; }

            set { _Descricao = value; }
        }
    }
}