using System.Collections.ObjectModel;

public class ClasseTiposAtividades
{

    public ObservableCollection<TipoAtividade> TiposAtividades = new ObservableCollection<TipoAtividade>();
    public class TipoAtividade
    {
        public int TipoAtividadeId { get; set; }
        public string Descricao { get; set; }
    }
}