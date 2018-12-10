// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System.Collections.ObjectModel;
using System.Windows.Input;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class VeiculoViewModel2 : BaseViewModel
    {
        private readonly IVeiculoService _veiculoService = new VeiculoService();

        #region  Propriedades

        public ObservableCollection<VeiculoViewModel2> Veiculos { get; set; }
        public Veiculo Veiculo { get; set; }

        public ICommand CriarCommand
        {
            get
            {
               //if(_veiculoService.Validar(Veiculo))
                var validade = _veiculoService.Validar (Veiculo);
                var d1 =  new RelayCommand(Criar);
                d1.CanExecute (validade);
                return d1;

            }
        }

        public ICommand AlterarCommand { get; set; }

        #endregion

        #region  Metodos

        public void Criar()
        {
            
            _veiculoService.Criar (Veiculo);
        }

        public void Alterar()
        {
            _veiculoService.Alterar (Veiculo);
        }

        //public void OnCriar()
        //{
        //    CriarCommand = new RelayCommand (Criar);
        //}

        //public void OnAlterar()
        //{
        //    CriarCommand = new RelayCommand (Alterar);
        //}

        #endregion
    }
}

//public string Nome { get; set; }

//private int _idade;
//public  int Idade
//{
//    get { return _idade; }
//    set
//    {
//        if(value==0)
//                throw new InvalidOperationException("A idade deve ser informada");
//        _idade = value;
//    }
//}