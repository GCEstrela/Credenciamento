using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseEquipamentoVeiculoTiposServicos
    {
        public ObservableCollection<EquipamentoVeiculoServico> EquipamentosVeiculosTiposServicos { get; set; }
        public class EquipamentoVeiculoServico
        {
            public int EquipamentoVeiculoTipoServicoID { get; set; }
            public int EquipamentoVeiculoID { get; set; }
            public int TipoServicoID { get; set; }
            public string Descricao { get; set; }

            public EquipamentoVeiculoServico CriaCopia(EquipamentoVeiculoServico equipamentoveiculoTiposServicos)
            {
                return (EquipamentoVeiculoServico)equipamentoveiculoTiposServicos.MemberwiseClone();
            }

        }
    }
}