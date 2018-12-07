using Genetec.Sdk;
using Genetec.Sdk.Workflows.EntityManager;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Activation;
using Genetec.Sdk.Entities.Builders;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Genetec.Sdk.Workspace.Components.BadgePrinter;
using Genetec.Sdk.Workspace.Services;
using Genetec.Sdk.Workspace;
using System.Printing;
using iModSCCredenciamento.Models;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Xml;
using System.Windows.Media.Imaging; 
using Genetec.Sdk.Entities.CustomFields;

namespace iModSCCredenciamento.Funcoes
{
    public class SCManager
    {
       //static IEngine _sdk = Main.engine;
        //public static bool ImprimirCredencial(ClasseColaboradoresCredenciais.ColaboradorCredencial colaboradorCredencial)
        //{
        //    try
        //    {

        //        //IEngine _sdk = Main.engine;

        //        Workspace m_workspace = PagePrincipalView.Workspace;

        //        bool _deletaCredencial = false;

        //        Cardholder _cardholder = _sdk.GetEntity((Guid)colaboradorCredencial.CardHolderGuid) as Cardholder;

        //        if (_cardholder == null)
        //        {
        //            return false;
        //        }

        //        Credential _credencial = _sdk.GetEntity((Guid)colaboradorCredencial.CredencialGuid) as Credential;

        //        if (_credencial == null)
        //        {
        //            _credencial =  CriarCredencialProvisoria(_cardholder, colaboradorCredencial.Validade, new Guid(colaboradorCredencial.LayoutCrachaGUID));
        //            _deletaCredencial = true;
        //        }

        //        Guid _CrachaGUID = new Guid(colaboradorCredencial.LayoutCrachaGUID);
        //        Guid _CHGUID = _credencial.CardholderGuid; // new Guid("227ee2c9-371f-408f-bf91-07cfb7ac8a74");

        //        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
        //        {
        //            PrintQueue printQueue = GetPrintQueue();
        //            if (printQueue != null)
        //            {

        //                IBadgeService badgeService = m_workspace.Services.Get<IBadgeService>();
        //                if (badgeService != null)
        //                {

        //                    BadgeInformation info = new BadgeInformation(_CrachaGUID, _credencial.Guid);
        //                    badgeService.BeginPrint(info, printQueue, OnBadgePrinted, null);

        //                }
        //            }
        //        }));

        //        if (_deletaCredencial)
        //        {
        //            _sdk.DeleteEntity(_credencial);
        //        }


        //        return true;



        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public static bool ImprimirCredencialVeiculo(ClasseVeiculosCredenciais.VeiculoCredencial veiculoCredencial)
        //{
        //    try
        //    {

        //        //IEngine _sdk = Main.engine;

        //        Workspace m_workspace = PagePrincipalView.Workspace;

        //        bool _deletaCredencial = false;

        //        Cardholder _cardholder = _sdk.GetEntity((Guid)veiculoCredencial.CardHolderGuid) as Cardholder;

        //        if (_cardholder == null)
        //        {
        //            return false;
        //        }

        //        Credential _credencial = _sdk.GetEntity((Guid)veiculoCredencial.CredencialGuid) as Credential;

        //        if (_credencial == null)
        //        {
        //            _credencial = CriarCredencialProvisoria(_cardholder, veiculoCredencial.Validade, new Guid(veiculoCredencial.LayoutCrachaGUID));
        //            _deletaCredencial = true;
        //        }

        //        Guid _CrachaGUID = new Guid(veiculoCredencial.LayoutCrachaGUID);
        //        Guid _CHGUID = _credencial.CardholderGuid; // new Guid("227ee2c9-371f-408f-bf91-07cfb7ac8a74");

        //        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
        //        {
        //            PrintQueue printQueue = GetPrintQueue();
        //            if (printQueue != null)
        //            {

        //                IBadgeService badgeService = m_workspace.Services.Get<IBadgeService>();
        //                if (badgeService != null)
        //                {

        //                    BadgeInformation info = new BadgeInformation(_CrachaGUID, _credencial.Guid);
        //                    badgeService.BeginPrint(info, printQueue, OnBadgePrinted, null);

        //                }
        //            }
        //        }));

        //        if (_deletaCredencial)
        //        {
        //            _sdk.DeleteEntity(_credencial);
        //        }


        //        return true;



        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //private static Credential CriarCredencialProvisoria(Cardholder cardholder, DateTime? validade, Guid layoutCracha)
        //{
        //    try
        //    {

        //        DateTime _DataValidade;

        //        if (validade != null)
        //        {
        //            _DataValidade = (DateTime)validade;

        //            _DataValidade = _DataValidade.AddSeconds(86399);

        //            _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
        //        }
        //        else
        //        {
        //            _DataValidade = DateTime.Now.AddSeconds(86399);
        //        }

        //        Credential _credencial;

        //            _sdk.TransactionManager.CreateTransaction();

        //            _credencial = _sdk.CreateEntity("Credencial de " + cardholder.FirstName, EntityType.Credential) as Credential;

        //            _credencial.Name = cardholder.FirstName + " " + cardholder.LastName;

        //            _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

        //            BadgeTemplate _BadgeTemplate = _sdk.GetEntity(layoutCracha) as BadgeTemplate;

        //            _credencial.BadgeTemplate = _BadgeTemplate.Guid;

        //            _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

        //            cardholder.Credentials.Add(_credencial);

        //            _sdk.TransactionManager.CommitTransaction();

        //        return _credencial;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        //private static void OnBadgePrinted(IAsyncResult ar)
        //{
        //    //throw new NotImplementedException();
        //}

        //private static PrintQueue GetPrintQueue()
        //{
        //    PrintQueue printQueue = null;
        //    bool finished = false;

        //    //  window.Dispatcher.BeginInvoke(new Action(() =>
        //    // {
        //    System.Windows.Controls.PrintDialog dlg = new System.Windows.Controls.PrintDialog();
        //    bool? bPrint = dlg.ShowDialog();
        //    if (bPrint.GetValueOrDefault())
        //    {
        //        printQueue = dlg.PrintQueue;
        //        try
        //        {
        //            System.Printing.ValidationResult result = printQueue.MergeAndValidatePrintTicket(printQueue.UserPrintTicket, dlg.PrintTicket);
        //            printQueue.UserPrintTicket = result.ValidatedPrintTicket;
        //            printQueue.Commit();
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    //      finished = true;
        //    //   }));

        //    //   while (!finished)
        //    //       Thread.Sleep(10);

        //    return printQueue;
        //}

        //public static bool Vincular(ClasseColaboradoresCredenciais.ColaboradorCredencial colaboradorCredencial)

        //{
        //    try
        //    {
        //        //IEngine _sdk = Main.engine;
        //        Cardholder _cardholder=null;
        //        string _firstname = "";
        //        string _lastname = "";
        //        ////// CardHolder
        //        try
        //        {

        //            _cardholder = BuscarCardHolder(colaboradorCredencial.CPF, colaboradorCredencial.CNPJ);
        //            _sdk.TransactionManager.CreateTransaction();

                    

        //            string[] _nomeCompleto = colaboradorCredencial.ColaboradorNome.Split(' ');

        //            int _len = _nomeCompleto.Count();

        //            if (_len > 1)
        //            {
        //                _lastname = _nomeCompleto[_len - 1];

        //                _firstname = _nomeCompleto[0];
        //            }
        //            else
        //            {
        //                _firstname = colaboradorCredencial.ColaboradorNome;
        //            }


        //            CardholderGroup _cardholderGroup = _sdk.GetEntity(EntityType.CardholderGroup, 1) as CardholderGroup;

        //            if (_cardholder == null)
        //            {
        //                _cardholder = _sdk.CreateEntity(colaboradorCredencial.ColaboradorNome, EntityType.Cardholder) as Cardholder;
        //            }

        //            BitmapImage _img1 = Conversores.STRtoIMG(colaboradorCredencial.ColaboradorFoto) as BitmapImage;

        //            BitmapImage _img2 = Conversores.STRtoIMG(colaboradorCredencial.EmpresaLogo) as BitmapImage;

        //            Bitmap _Foto = Conversores.BitmapImageToBitmap(_img1);

        //            if (_Foto != null)
        //            {
        //                _cardholder.Picture = _Foto;
        //            }

        //            //////   CUSTOM FIELDS //////

        //            //var _systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

        //            //var _customFieldService = _systemConfiguration.CustomFieldService;

        //            //Bitmap _Logo = Conversores.BitmapImageToBitmap(_img2);

        //            //System.Drawing.Image _img3 = System.Drawing.Image.FromFile("D:\\Meus Documentos\\Visual Studio 2015\\Projects\\Genetec\\Projeto Credenciamento\\iModSCCredenciamento 1.0.0.3\\iModSCCredenciamento\\Resources\\Contrato.jpg");

        //            //var _customField = _customFieldService.CustomFields.Where(x => x.EntityType == EntityType.Cardholder && x.Name.Equals("Logo")).FirstOrDefault();

        //            //if (_customField != null)
        //            //{
        //            //    _customFieldService.SetValue(_customField, _cardholder.Guid, _img3);
        //            //}


        //            Bitmap _Logo = Conversores.BitmapImageToBitmap(_img2);

        //            if (_Logo != null)
        //            {
        //                _cardholder.SetCustomFieldAsync("Logo", _Logo);
        //            }

        //            Bitmap _Motorista = null;

        //            if (colaboradorCredencial.Motorista)
        //            {
        //                _img1 = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Veiculo.png", UriKind.Absolute));
        //                _Motorista = Conversores.BitmapImageToBitmap(_img1);
        //                _cardholder.SetCustomFieldAsync("Motorista", _Motorista);
        //            }

        //            _cardholder.SetCustomFieldAsync("Colaborador (ID)", colaboradorCredencial.ColaboradorID);

        //            _cardholder.SetCustomFieldAsync("Apelido", colaboradorCredencial.ColaboradorApelido.Trim());

        //            _cardholder.SetCustomFieldAsync("CPF", colaboradorCredencial.CPF.Trim());

        //            _cardholder.SetCustomFieldAsync("Empresa", colaboradorCredencial.EmpresaNome.Trim());

        //            _cardholder.SetCustomFieldAsync("Nome Fantasia", colaboradorCredencial.EmpresaApelido.Trim());

        //            _cardholder.SetCustomFieldAsync("CNPJ", colaboradorCredencial.CNPJ.Trim());

        //            _cardholder.SetCustomFieldAsync("Cargo", colaboradorCredencial.Cargo.Trim());



        //            _cardholder.InsertIntoPartition(Partition.DefaultPartitionGuid);

        //            if (_cardholder.Groups.Count == 0 && _cardholderGroup != null)
        //            {
        //                _cardholder.Groups.Add(_cardholderGroup.Guid);
        //            }


        //            _cardholder.FirstName = _firstname.Trim();

        //            _cardholder.LastName = _lastname.Trim();

        //            _sdk.TransactionManager.CommitTransaction();

        //            colaboradorCredencial.CardHolderGuid = _cardholder.Guid;
        //        }
        //        catch (Exception ex)
        //        {
        //            if (_sdk.TransactionManager.IsTransactionActive) { _sdk.TransactionManager.RollbackTransaction(); }
                    
        //            return false;
        //        }



        //        //// Credencial
        //        ///
        //        if (colaboradorCredencial.FormatIDGUID != "00000000-0000-0000-0000-000000000000" && colaboradorCredencial.NumeroCredencial != "")
        //        {

        //            try
        //            {

        //                DateTime _DataValidade;

        //                if (colaboradorCredencial.Validade != null)
        //                {
        //                    _DataValidade = (DateTime)colaboradorCredencial.Validade;

        //                    _DataValidade = _DataValidade.AddSeconds(86399);

        //                    _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
        //                }
        //                else
        //                {
        //                    _DataValidade = DateTime.Now.AddSeconds(86399);
        //                }

        //                Credential _credencial; // = _sdk.GetEntity((Guid)colaboradorCredencial.CredencialGuid) as Credential;

        //                _credencial = BuscarCredencial( colaboradorCredencial.NumeroCredencial, colaboradorCredencial.FormatIDGUID, colaboradorCredencial.FC);

                        
        //                if (_credencial != null)
        //                {
        //                    if (_credencial.CardholderGuid != _cardholder.Guid)
        //                    {
        //                        //MessageBox.Show("Esta credencial pertence a outro usuário e não pode ser vinculada!", "Erro ao Vincular", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        //                        Global.PopupBox("Esta credencial já está associada a um usuário e não pode ser vinculada!", 4);

        //                        return false;
        //                    }
        //                    else // atualizar credencial
        //                    {
        //                        if (colaboradorCredencial.LayoutCrachaGUID != "")
        //                        {
        //                            //BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(colaboradorCredencial.LayoutCrachaGUID)) as BadgeTemplate;
        //                            //_credencial.BadgeTemplate = _BadgeTemplate.Guid;

        //                            _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

        //                            _credencial.SetCustomFieldAsync("Privilégio #1", colaboradorCredencial.PrivilegioDescricao1);

        //                            _credencial.SetCustomFieldAsync("Privilégio #2", colaboradorCredencial.PrivilegioDescricao2);
        //                        }

        //                    }
        //                }
        //                else //criar nova credencial
        //                {


        //                    _sdk.TransactionManager.CreateTransaction();

        //                    _credencial = _sdk.CreateEntity("Credencial de " + _firstname, EntityType.Credential) as Credential;

        //                    _credencial.Name = colaboradorCredencial.NumeroCredencial.Trim() + " - " + _firstname + " " + _lastname;

        //                    _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);


        //                    if (!colaboradorCredencial.Ativa)
        //                    {
        //                        _credencial.State = CredentialState.Inactive;
        //                    }


        //                    //_credencial.SetCustomFieldAsync("Privilégio", colaboradorCredencial.PrivilegioDescricao1);

        //                    if (colaboradorCredencial.LayoutCrachaGUID != "")
        //                    {
        //                        //BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(colaboradorCredencial.LayoutCrachaGUID)) as BadgeTemplate;
        //                        //_credencial.BadgeTemplate = _BadgeTemplate.Guid;

        //                    }

        //                    //0	N/D                           	00000000-0000-0000-0000-000000000000
        //                    //1	Standard - 26 bits            	00000000-0000-0000-0000-000000000200
        //                    //2	H10302 - 37 bits              	00000000-0000-0000-0000-000000000400
        //                    //3	H10304 - 37 bits              	00000000-0000-0000-0000-000000000500
        //                    //4	H10306 - 34 bits              	00000000-0000-0000-0000-000000000300
        //                    //5	HID Corporate 1000 - 35 bits  	00000000-0000-0000-0000-000000000600
        //                    //6	HID Corporate 1000 - 48 bits  	00000000-0000-0000-0000-000000000800
        //                    //7	CSN                           	00000000-0000-0000-0000-000000000700


        //                    switch (colaboradorCredencial.FormatIDGUID)
        //                    {
        //                        case "00000000-0000-0000-0000-000000000200":
        //                            _credencial.Format = new WiegandStandardCredentialFormat(Convert.ToInt32(colaboradorCredencial.FC), Convert.ToInt32(colaboradorCredencial.NumeroCredencial.Trim()));
        //                            break;
        //                        case "00000000-0000-0000-0000-000000000400":
        //                            _credencial.Format = new WiegandH10302CredentialFormat(Convert.ToInt32(colaboradorCredencial.NumeroCredencial.Trim()));
        //                            break;
        //                        case "00000000-0000-0000-0000-000000000500":
        //                            _credencial.Format = new WiegandH10304CredentialFormat(Convert.ToInt32(colaboradorCredencial.FC), Convert.ToInt32(colaboradorCredencial.NumeroCredencial.Trim()));
        //                            break;
        //                        case "00000000-0000-0000-0000-000000000300":
        //                            _credencial.Format = new WiegandH10306CredentialFormat(Convert.ToInt32(colaboradorCredencial.FC), Convert.ToInt32(colaboradorCredencial.NumeroCredencial.Trim()));
        //                            break;
        //                        case "00000000-0000-0000-0000-000000000600":
        //                            _credencial.Format = new WiegandCorporate1000CredentialFormat(Convert.ToInt32(colaboradorCredencial.FC), Convert.ToInt32(colaboradorCredencial.NumeroCredencial.Trim()));
        //                            break;
        //                        case "00000000-0000-0000-0000-000000000800":
        //                            _credencial.Format = new Wiegand48BitCorporate1000CredentialFormat(Convert.ToInt32(colaboradorCredencial.FC), Convert.ToInt32(colaboradorCredencial.NumeroCredencial.Trim()));
        //                            break;

        //                        case "00000000-0000-0000-0000-000000000700":
        //                            long _cardnumber = long.Parse(colaboradorCredencial.NumeroCredencial.Trim());
        //                            _credencial.Format = new WiegandCsn32CredentialFormat(_cardnumber);
        //                            break;
        //                    }

        //                    //if (_credencial.Format != null)
        //                    //{


        //                        _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

        //                        _cardholder.Credentials.Add(_credencial);
        //                    //}


        //                    _sdk.TransactionManager.CommitTransaction();

                            
        //                }

        //                colaboradorCredencial.CredencialGuid = _credencial.Guid;

        //            }
        //            catch (Exception ex)
        //            {
        //                if (_sdk.TransactionManager.IsTransactionActive) { _sdk.TransactionManager.RollbackTransaction(); }
        //                return false;
        //            }
        //        }

        //        return true;
        //    }

        //    catch (Exception ex)
        //    {

        //        return false;
        //    }


        //}

        public static bool VincularVeiculo(ClasseVeiculosCredenciais.VeiculoCredencial veiculoCredencial)

        {
            return true;
            //try
            //{
            //    //IEngine _sdk = Main.engine;
            //    Cardholder _cardholder;
            //    string _firstname = "";
            //    string _lastname = "";
            //    ////// CardHolder
            //    try
            //    {
            //        _sdk.TransactionManager.CreateTransaction();

            //        _cardholder = BuscarCardHolder(veiculoCredencial.Placa, veiculoCredencial.CNPJ);

            //        string[] _nomeCompleto = veiculoCredencial.VeiculoNome.Split(' ');

            //        int _len = _nomeCompleto.Count();

            //        if (_len > 1)
            //        {
            //            _lastname = _nomeCompleto[_len - 1];

            //            _firstname = _nomeCompleto[0];
            //        }
            //        else
            //        {
            //            _firstname = veiculoCredencial.VeiculoNome;
            //        }

            //        //DateTime _DataValidade;

            //        //if (vinculo.Validade != null)
            //        //{
            //        //    _DataValidade = (DateTime)vinculo.Validade;

            //        //    _DataValidade = _DataValidade.AddSeconds(86399);

            //        //    _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
            //        //}
            //        //else
            //        //{
            //        //    _DataValidade = DateTime.Now.AddSeconds(86399);
            //        //}


            //        CardholderGroup _cardholderGroup = _sdk.GetEntity(EntityType.CardholderGroup, 1) as CardholderGroup;

            //        if (_cardholder == null)
            //        {
            //            _cardholder = _sdk.CreateEntity(veiculoCredencial.VeiculoNome, EntityType.Cardholder) as Cardholder;
            //        }

            //        BitmapImage _img = Conversores.STRtoIMG(veiculoCredencial.VeiculoFoto) as BitmapImage;

            //        Bitmap _Foto = Conversores.BitmapImageToBitmap(_img);


            //        if (_Foto != null)
            //        {
            //            _cardholder.Picture = _Foto;
            //        }

            //        Bitmap _Motorista = null;


            //        _cardholder.SetCustomFieldAsync("No. do Veiculo", veiculoCredencial.VeiculoID);

            //        _cardholder.SetCustomFieldAsync("CPF", veiculoCredencial.Placa);

            //        _cardholder.SetCustomFieldAsync("Motorista", _img);

            //        _cardholder.SetCustomFieldAsync("Empresa", veiculoCredencial.EmpresaNome);

            //        _cardholder.SetCustomFieldAsync("Nome Fantasia", veiculoCredencial.EmpresaApelido);

            //        _cardholder.SetCustomFieldAsync("CNPJ", veiculoCredencial.CNPJ);

            //        _cardholder.SetCustomFieldAsync("Cargo", veiculoCredencial.Cargo);

            //        _cardholder.InsertIntoPartition(Partition.DefaultPartitionGuid);

            //        if (_cardholder.Groups.Count == 0 && _cardholderGroup != null)
            //        {
            //            _cardholder.Groups.Add(_cardholderGroup.Guid);
            //        }


            //        _cardholder.FirstName = _firstname;

            //        _cardholder.LastName = _lastname;


            //        //_cardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

            //        _sdk.TransactionManager.CommitTransaction();

            //        veiculoCredencial.CardHolderGuid = _cardholder.Guid;
            //    }
            //    catch (Exception ex)
            //    {

            //        return false;
            //    }



            //// Credencial
            ///
            //    if (veiculoCredencial.FormatIDGUID != "00000000-0000-0000-0000-000000000000" && veiculoCredencial.NumeroCredencial != "")
            //    {

            //        try
            //        {

            //            DateTime _DataValidade;

            //            if (veiculoCredencial.Validade != null)
            //            {
            //                _DataValidade = (DateTime)veiculoCredencial.Validade;

            //                _DataValidade = _DataValidade.AddSeconds(86399);

            //                _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
            //            }
            //            else
            //            {
            //                _DataValidade = DateTime.Now.AddSeconds(86399);
            //            }

            //            Credential _credencial; // = _sdk.GetEntity((Guid)veiculoCredencial.CredencialGuid) as Credential;

            //            _credencial = BuscarCredencial(veiculoCredencial.NumeroCredencial, veiculoCredencial.FormatIDGUID, veiculoCredencial.FC);


            //            if (_credencial != null)
            //            {
            //                if (_credencial.CardholderGuid != _cardholder.Guid)
            //                {
            //                    //MessageBox.Show("Esta credencial pertence a outro usuário e não pode ser vinculada!", "Erro ao Vincular", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            //                    Global.PopupBox("Esta credencial já está associada a um usuário e não pode ser vinculada!", 4);

            //                    return false;
            //                }
            //                else // atualizar credencial
            //                {
            //                    if (veiculoCredencial.LayoutCrachaGUID != "")
            //                    {
            //                        BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(veiculoCredencial.LayoutCrachaGUID)) as BadgeTemplate;
            //                        _credencial.BadgeTemplate = _BadgeTemplate.Guid;

            //                        _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);
            //                    }

            //                }
            //            }
            //            else //criar nova credencial
            //            {


            //                _sdk.TransactionManager.CreateTransaction();

            //                _credencial = _sdk.CreateEntity("Credencial de " + _firstname, EntityType.Credential) as Credential;

            //                _credencial.Name = veiculoCredencial.NumeroCredencial + " - " + _firstname + " " + _lastname;

            //                _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

            //                if (veiculoCredencial.LayoutCrachaGUID != "")
            //                {
            //                    BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(veiculoCredencial.LayoutCrachaGUID)) as BadgeTemplate;
            //                    _credencial.BadgeTemplate = _BadgeTemplate.Guid;

            //                }

            //                //0	N/D                           	00000000-0000-0000-0000-000000000000
            //                //1	Standard - 26 bits            	00000000-0000-0000-0000-000000000200
            //                //2	H10302 - 37 bits              	00000000-0000-0000-0000-000000000400
            //                //3	H10304 - 37 bits              	00000000-0000-0000-0000-000000000500
            //                //4	H10306 - 34 bits              	00000000-0000-0000-0000-000000000300
            //                //5	HID Corporate 1000 - 35 bits  	00000000-0000-0000-0000-000000000600
            //                //6	HID Corporate 1000 - 48 bits  	00000000-0000-0000-0000-000000000800
            //                //7	CSN                           	00000000-0000-0000-0000-000000000700


            //                switch (veiculoCredencial.FormatIDGUID)
            //                {
            //                    case "00000000-0000-0000-0000-000000000200":
            //                        _credencial.Format = new WiegandStandardCredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
            //                        break;
            //                    case "00000000-0000-0000-0000-000000000400":
            //                        _credencial.Format = new WiegandH10302CredentialFormat(Convert.ToInt32(veiculoCredencial.NumeroCredencial));
            //                        break;
            //                    case "00000000-0000-0000-0000-000000000500":
            //                        _credencial.Format = new WiegandH10304CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
            //                        break;
            //                    case "00000000-0000-0000-0000-000000000300":
            //                        _credencial.Format = new WiegandH10306CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
            //                        break;
            //                    case "00000000-0000-0000-0000-000000000600":
            //                        _credencial.Format = new WiegandCorporate1000CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
            //                        break;
            //                    case "00000000-0000-0000-0000-000000000800":
            //                        _credencial.Format = new Wiegand48BitCorporate1000CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
            //                        break;

            //                    case "00000000-0000-0000-0000-000000000700":
            //                        CustomCredentialFormat mifareCSN;

            //                        SystemConfiguration sysConfig = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

            //                        if (sysConfig != null)
            //                        {


            //                            foreach (CredentialFormat cardFormat in sysConfig.CredentialFormats)
            //                            {
            //                                if (cardFormat.Name == "CSN")
            //                                {
            //                                    mifareCSN = cardFormat as CustomCredentialFormat;
            //                                    mifareCSN.SetValues(long.Parse(veiculoCredencial.NumeroCredencial));
            //                                    _credencial.Format = mifareCSN;
            //                                    break;
            //                                }
            //                                //
            //                            }
            //                        }
            //                        break;
            //                }

            //                //if (_credencial.Format != null)
            //                //{


            //                _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

            //                _cardholder.Credentials.Add(_credencial);
            //                //}


            //                _sdk.TransactionManager.CommitTransaction();


            //            }

            //            veiculoCredencial.CredencialGuid = _credencial.Guid;

            //        }
            //        catch (Exception ex)
            //        {

            //            return false;
            //        }
            //    }

            //    return true;
            //}

            //catch (Exception ex)
            //{

            //    return false;
            //}


        }


        //private static Cardholder BuscarCardHolder(string _CPF, string _CNPJ)
        //{
        //    EntityConfigurationQuery query;

        //    QueryCompletedEventArgs result;

        //    try
        //    {
        //        query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

        //        query.EntityTypeFilter.Add(EntityType.Cardholder);

        //        query.NameSearchMode = StringSearchMode.StartsWith;

        //        result = query.Query();

        //        SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

        //        var service = systemConfiguration.CustomFieldService;

        //        if (result.Success)
        //        {
        //            foreach (DataRow dr in result.Data.Rows)
        //            {
        //                Cardholder _cardholder = _sdk.GetEntity((Guid)dr[0]) as Cardholder;

        //                string _CPFteste = service.GetValue<string>("CPF", _cardholder.Guid);

        //                string _CNPJteste = service.GetValue<string>("CNPJ", _cardholder.Guid);

        //                if (_CPFteste == _CPF && _CNPJteste == _CNPJ)
        //                {
        //                    return _cardholder;
        //                }
        //            }

        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void BuscarCardHolder ex: " + ex);

        //        return null;
        //    }
        //}

            //0	N/D                           	00000000-0000-0000-0000-000000000000
            //1	Standard - 26 bits            	00000000-0000-0000-0000-000000000200
            //2	H10302 - 37 bits              	00000000-0000-0000-0000-000000000400
            //3	H10304 - 37 bits              	00000000-0000-0000-0000-000000000500
            //4	H10306 - 34 bits              	00000000-0000-0000-0000-000000000300
            //5	HID Corporate 1000 - 35 bits  	00000000-0000-0000-0000-000000000600
            //6	HID Corporate 1000 - 48 bits  	00000000-0000-0000-0000-000000000800
            //7	CSN                           	00000000-0000-0000-0000-000000000700


        //private static Credential BuscarCredencial( string _NumeroCredencial, string _FormatoCredencial, int _FC =0)
        //{
        //    EntityConfigurationQuery query;

        //    QueryCompletedEventArgs result;

        //    try
        //    {
        //        query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

        //        query.EntityTypeFilter.Add(EntityType.Credential);
                
        //        query.NameSearchMode = StringSearchMode.StartsWith;

        //        result = query.Query();

        //        if (result.Success)
        //        {
        //            foreach (DataRow dr in result.Data.Rows)
        //            {
        //                Credential _credencial = _sdk.GetEntity((Guid)dr[0]) as Credential;

        //                string _NumeroCredencialteste;
        //                int _FCteste;
        //                string _FormatoCredencialTeste = _credencial.Format.FormatId.ToString();
        //                if (_FormatoCredencial== _FormatoCredencialTeste)
        //                {

        //                    switch (_FormatoCredencialTeste)
        //                    {
        //                        case "00000000-0000-0000-0000-000000000200":
        //                        case "00000000-0000-0000-0000-000000000300":
        //                        case "00000000-0000-0000-0000-000000000500":
        //                        case "00000000-0000-0000-0000-000000000600":
        //                        case "00000000-0000-0000-0000-000000000800":
        //                            _FCteste = ((Genetec.Sdk.Credentials.WiegandStandardCredentialFormat)_credencial.Format).Facility;

        //                            _NumeroCredencialteste = ((Genetec.Sdk.Credentials.WiegandCredentialFormat)_credencial.Format).CardId.ToString();

        //                            if (_FCteste == _FC && _NumeroCredencialteste == _NumeroCredencial)
        //                            {
        //                                return _credencial;
        //                            }

        //                            break;

        //                        case "00000000-0000-0000-0000-000000000400":
        //                        case "00000000-0000-0000-0000-000000000700":
        //                            _NumeroCredencialteste = ((Genetec.Sdk.Credentials.WiegandCredentialFormat)_credencial.Format).CardId.ToString();
        //                            //_NumeroCredencialteste = long.Parse(_NumeroCredencialteste, System.Globalization.NumberStyles.HexNumber);

        //                            if (_NumeroCredencialteste == _NumeroCredencial)
        //                            {
        //                                return _credencial;
        //                            }
        //                            break;
        //                    }

        //                }

        //            }

        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void BuscarCredencial ex: " + ex);

        //        return null;
        //    }
        //}

        public static bool ExcluirCredencial(Guid? _CredencialGuid)
        {
            //try
            //{

            //    if (_CredencialGuid == null || _CredencialGuid == new Guid("00000000-0000-0000-0000-000000000000"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        Credential _Credencial = _sdk.GetEntity((Guid)_CredencialGuid) as Credential;

            //        if (_Credencial!= null)
            //        {
            //            _sdk.DeleteEntity(_Credencial);
            //        }
                    
            //    }
            //}
            //catch (Exception ex)
            //{

            //    return false;
            //}

            return true;
        }


        //#region Testes
        //public static void TesteCredencial()
        //{
        //    //IEngine _sdk = Main.engine;
        //    EntityConfigurationQuery query;
        //    QueryCompletedEventArgs result;
        //    try
        //    {
        //        query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
        //        query.EntityTypeFilter.Add(EntityType.Credential);

        //        query.NameSearchMode = StringSearchMode.StartsWith;
        //        result = query.Query();

        //        if (result.Success)
        //        {
        //            foreach (DataRow dr in result.Data.Rows)
        //            {
        //                Credential _credencial = _sdk.GetEntity((Guid)dr[0]) as Credential;

        //            }

        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void BuscarCardHolder ex: " + ex);
               
        //    }
        //}
        //#endregion

    }

}
