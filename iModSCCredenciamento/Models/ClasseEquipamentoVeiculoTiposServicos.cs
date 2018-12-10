using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseEquipamentoVeiculoTiposServicos
    {
        public ObservableCollection<EquipamentoVeiculoServico> EquipamentosVeiculosTiposServicos { get; set; }
        public class EquipamentoVeiculoServico
        {
            public int EquipamentoVeiculoTipoServicoId { get; set; }
            public int EquipamentoVeiculoId { get; set; }
            public int? TipoServicoId { get; set; }
            public string Descricao { get; set; }

            public EquipamentoVeiculoServico CriaCopia(EquipamentoVeiculoServico equipamentoveiculoTiposServicos)
            {
                return (EquipamentoVeiculoServico)equipamentoveiculoTiposServicos.MemberwiseClone();
            }

        }
    }
}