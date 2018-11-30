using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasEquipamentos
    {
        public ObservableCollection<EmpresaEquipamento> EmpresasEquipamentos { get; set; }
        public class EmpresaEquipamento
        {

            public int EmpresaEquipamentoID { get; set; }
            public string Descricao { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Ano { get; set; }
            public string Patrimonio { get; set; }
            public string Seguro { get; set; }
            public string ApoliceSeguro { get; set; }
            public string ApoliceValor { get; set; }
            public string ApoliceVigencia { get; set; }
            public DateTime? DataEmissao { get; set; }
            public DateTime? DataValidade { get; set; }
            public string Excluido { get; set; }
            public int TipoEquipamentoID { get; set; }
            public int StatusID { get; set; }
            public int TipoAcessoID { get; set; }
            public int EmpresaID { get; set; }

            public EmpresaEquipamento CriaCopia(EmpresaEquipamento _EmpresasEquipamentos)
            {
                return (EmpresaEquipamento)_EmpresasEquipamentos.MemberwiseClone();
            }
        }
    }
}
