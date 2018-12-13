using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasAreasAcessos
    {

        public ObservableCollection<EmpresaAreaAcesso> EmpresasAreasAcessos { get; set; }
        public class EmpresaAreaAcesso
        {
            public int EmpresaAreaAcessoID { get; set; }
            public int EmpresaID { get; set; }
            public int AreaAcessoID { get; set; }
            public string Descricao { get; set; }
            public string Identificacao { get; set; }

            public EmpresaAreaAcesso CriaCopia(EmpresaAreaAcesso empresaAreasAcessos)
            {
                return (EmpresaAreaAcesso)empresaAreasAcessos.MemberwiseClone();
            }

        }
    }
}
