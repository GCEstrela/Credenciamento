using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoMapper;
using iModSCCredenciamento.Models;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using Colaborador = IMOD.Domain.Entities.Colaborador;

namespace iModSCCredenciamento
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //AutoMapperConfig.RegisterMappings();
        }
    }


    public class AutoMapperConfig
    {

        public static void RegisterMappings()
        {
            Mapper.Initialize(
                m =>
                {
                    m.CreateMap<Colaborador, ClasseColaboradores.Colaborador>().ReverseMap();
                    m.CreateMap<Empresa, ClasseEmpresas.Empresa>()
                        .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                        .ReverseMap();

                    m.CreateMap<Estados, ClasseEstados.Estado>().ReverseMap();

                    m.CreateMap<TipoAtividade, ClasseTiposAtividades.TipoAtividade>().ReverseMap();

                    m.CreateMap<EmpresaTipoAtividade, ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>().ReverseMap();

                    m.CreateMap<AreaAcesso, ClasseAreasAcessos.AreaAcesso>().ReverseMap();
                    m.CreateMap<LayoutCracha, ClasseLayoutsCrachas.LayoutCracha>().ReverseMap();
                    m.CreateMap<EmpresaLayoutCrachaView, ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>().ReverseMap();

                });
        }


    }
}
