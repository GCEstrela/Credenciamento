using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Windows.Controls;
using Genetec.Sdk;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Activation;
using Genetec.Sdk.Queries;
using IMOD.CrossCutting;
//using iModSCCredenciamento.Models;
using ValidationResult = System.Printing.ValidationResult;

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class SCManager
    {
        static IEngine _sdk = Main.Engine;
        //public static bool ImprimirCredencial(ClasseColaboradoresCredenciais.ColaboradorCredencial colaboradorCredencial)
        //{
        //    try
        //    {

        //        //IEngine _sdk = Main.engine;

        //        Workspace m_workspace = PagePrincipalView.Workspace;

        //        bool _deletaCredencial = false;

        //        //Cardholder _cardholder = _sdk.GetEntity((Guid)colaboradorCredencial.CardHolderGuid) as Cardholder;

        //        //if (_cardholder == null)
        //        //{
        //        //    return false;
        //        //}

        //        //Credential _credencial = _sdk.GetEntity((Guid)colaboradorCredencial.CredencialGuid) as Credential;

        //        //if (_credencial == null)
        //        //{
        //        //    _credencial =  CriarCredencialProvisoria(_cardholder, colaboradorCredencial.Validade, new Guid(colaboradorCredencial.LayoutCrachaGUID));
        //        //    _deletaCredencial = true;
        //        //}

        //        Guid _CrachaGUID = new Guid(colaboradorCredencial.LayoutCrachaGUID);
        //        //Guid _CHGUID = _credencial.CardholderGuid; // new Guid("227ee2c9-371f-408f-bf91-07cfb7ac8a74");

        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            PrintQueue printQueue = GetPrintQueue();
        //            if (printQueue != null)
        //            {

        //                IBadgeService badgeService = m_workspace.Services.Get<IBadgeService>();
        //                if (badgeService != null)
        //                {

        //                    //BadgeInformation info = new BadgeInformation(_CrachaGUID, _credencial.Guid);
        //                    //badgeService.BeginPrint(info, printQueue, OnBadgePrinted, null);

        //                }
        //            }
        //        });

        //        if (_deletaCredencial)
        //        {
        //          //  _sdk.DeleteEntity(_credencial);
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

        //        Application.Current.Dispatcher.Invoke(() =>
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
        //        });

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

        private static Credential CriarCredencialProvisoria(Cardholder cardholder, DateTime? validade, Guid layoutCracha)
        {
            IEngine _sdk = Main.Engine;
            DateTime _DataValidade;

            if (validade != null)
            {
                _DataValidade = (DateTime)validade;

                _DataValidade = _DataValidade.AddSeconds(86399);

                _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
            }
            else
            {
                _DataValidade = DateTime.Now.AddSeconds(86399);
            }

            Credential _credencial;

            _sdk.TransactionManager.CreateTransaction();

            _credencial = _sdk.CreateEntity("Credencial de " + cardholder.FirstName, EntityType.Credential) as Credential;

            _credencial.Name = cardholder.FirstName + " " + cardholder.LastName;

            _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

            BadgeTemplate _BadgeTemplate = _sdk.GetEntity(layoutCracha) as BadgeTemplate;

            _credencial.BadgeTemplate = _BadgeTemplate.Guid;

            _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

            cardholder.Credentials.Add(_credencial);

            _sdk.TransactionManager.CommitTransaction();

            return _credencial;
        }
        public Boolean CredencialStatus(string cardholder, string credential, Boolean Ativado, int status)
        {
            _sdk.TransactionManager.CreateTransaction();

            if (Ativado)
            {
                Cardholder _cardholder = _sdk.GetEntity(new Guid(cardholder)) as Cardholder;
                _cardholder.State = CardholderState.Active;

                Credential _credential = _sdk.GetEntity(new Guid(credential)) as Credential;
                _credential.State = CredentialState.Active;
            }
            else
            {

                Cardholder _cardholder = _sdk.GetEntity(new Guid(cardholder)) as Cardholder;
                _cardholder.State = CardholderState.Inactive;

                switch (status)
                {

                    case 6:
                        Credential _credential = _sdk.GetEntity(new Guid(credential)) as Credential;
                        _credential.State = CredentialState.Expired;
                        break;
                    case 10:
                        Credential _credential2 = _sdk.GetEntity(new Guid(credential)) as Credential;
                        _credential2.State = CredentialState.Stolen;
                        break;
                    default:
                        Credential _credential3 = _sdk.GetEntity(new Guid(credential)) as Credential;
                        _credential3.State = CredentialState.Inactive;
                        break;
                }

            }

            _sdk.TransactionManager.CommitTransaction();

            return true;

        }
        private static void OnBadgePrinted(IAsyncResult ar)
        {
            //throw new NotImplementedException();
        }

        private static PrintQueue GetPrintQueue()
        {
            PrintQueue printQueue = null;
            bool finished = false;

            //  window.Dispatcher.BeginInvoke(new Action(() =>
            // {
            PrintDialog dlg = new PrintDialog();
            bool? bPrint = dlg.ShowDialog();
            if (bPrint.GetValueOrDefault())
            {
                printQueue = dlg.PrintQueue;
                try
                {
                    ValidationResult result = printQueue.MergeAndValidatePrintTicket(printQueue.UserPrintTicket, dlg.PrintTicket);
                    printQueue.UserPrintTicket = result.ValidatedPrintTicket;
                    printQueue.Commit();
                }
                catch (Exception ex)
                {

                }
            }
            //      finished = true;
            //   }));

            //   while (!finished)
            //       Thread.Sleep(10);

            return printQueue;
        }

        public string Vincular(string _Nome, string _CPF, string _CNPJ, string _Empresa, string _Matricula, string _Cargo,
                                            string _FC, string _NumeroCredencial, string _FormatoCredencial, string _DataValidadeSTR, string _LayoutCrachaGuid = "", Bitmap _Foto = null)
        {
            try
            {
                //IEngine _sdk = Main.engine;
                Cardholder _cardholder;
                string _firstname = "";
                string _lastname = "";
                string _cardholderGuid = "";
                string _credencialGuid = "";
                ////// CardHolder
                try
                {
                    _cardholder = BuscarCardHolder(_CPF, _CNPJ);

                    string[] _nomeCompleto = _Nome.Split(' ');

                    int _len = _nomeCompleto.Count();

                    if (_len > 1)
                    {
                        _lastname = _nomeCompleto[_len - 1];

                        _firstname = _nomeCompleto[0];
                    }
                    else
                    {
                        _firstname = _Nome;
                    }
                    DateTime _DataValidade = DateTime.Now.AddSeconds(86399);

                    if (_DataValidadeSTR != "")
                    {
                        _DataValidade = Convert.ToDateTime(_DataValidadeSTR);

                        _DataValidade = _DataValidade.AddSeconds(86399);

                        _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
                    }

                    _sdk.TransactionManager.CreateTransaction();

                    CardholderGroup _cardholderGroup;

                    if (_cardholder == null)
                    {
                        _cardholderGroup = _sdk.GetEntity(EntityType.CardholderGroup, 1) as CardholderGroup;

                        _cardholder = _sdk.CreateEntity(_Nome, EntityType.Cardholder) as Cardholder;

                        _cardholder.SetCustomFieldAsync("CPF", _CPF);

                        _cardholder.SetCustomFieldAsync("Empresa", _Empresa);

                        _cardholder.SetCustomFieldAsync("CNPJ", _CNPJ);

                        _cardholder.SetCustomFieldAsync("Matricula", _Matricula);

                        _cardholder.SetCustomFieldAsync("Cargo", _Cargo);

                        _cardholder.InsertIntoPartition(Partition.DefaultPartitionGuid);

                        if (_cardholderGroup != null)
                        {
                            _cardholder.Groups.Add(_cardholderGroup.Guid);
                        }
                    }

                    _cardholder.FirstName = _firstname;

                    _cardholder.LastName = _lastname;


                    _cardholder.SetCustomFieldAsync("CPF", _CPF);

                    _cardholder.SetCustomFieldAsync("Empresa", _Empresa);

                    _cardholder.SetCustomFieldAsync("CNPJ", _CNPJ);

                    _cardholder.SetCustomFieldAsync("Matricula", _Matricula);

                    _cardholder.SetCustomFieldAsync("Cargo", _Cargo);





                    if (_Foto != null)
                    {
                        _cardholder.Picture = _Foto;
                    }

                    _cardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);
                    _cardholderGuid = _cardholder.Guid.ToString();

                    _sdk.TransactionManager.CommitTransaction();

                    //return true;
                }
                catch (Exception ex)
                {

                    return null;
                }

                //// Credencial
                try
                {
                    Credential _credencial;

                    _credencial = BuscarCredencial(_FC, _NumeroCredencial);

                    if (_credencial != null)
                    {
                        if (_credencial.CardholderGuid != _cardholder.Guid)
                        {
                            //MessageBox.Show("Esta credencial pertence a outro usuário e não pode ser vinculada!", "Erro ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return null;
                        }
                        else // atualizar credencial
                        {
                            if (_LayoutCrachaGuid != "")
                            {
                                BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(_LayoutCrachaGuid)) as BadgeTemplate;
                                _credencial.BadgeTemplate = _BadgeTemplate.Guid;
                            }

                        }
                    }
                    else //criar nova credencial
                    {
                        _sdk.TransactionManager.CreateTransaction();

                        _credencial = _sdk.CreateEntity(_NumeroCredencial, EntityType.Credential) as Credential;

                        _credencial.Name = _NumeroCredencial + " - " + _firstname + " " + _lastname;

                        if (_LayoutCrachaGuid != "")
                        {
                            BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(_LayoutCrachaGuid)) as BadgeTemplate;
                            _credencial.BadgeTemplate = _BadgeTemplate.Guid;

                        }



                        switch (_FormatoCredencial)
                        {
                            case "Standard 26 bits":
                                _credencial.Format = new WiegandStandardCredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                                break;
                            case "H10302":
                                _credencial.Format = new WiegandH10302CredentialFormat(Convert.ToInt32(_NumeroCredencial));
                                break;
                            case "H10304":
                                _credencial.Format = new WiegandH10304CredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                                break;
                            case "H10306":
                                _credencial.Format = new WiegandH10306CredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                                break;
                            case "HID Corporate 1000":
                                _credencial.Format = new WiegandCorporate1000CredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                                break;

                            case "CSN":
                                CustomCredentialFormat mifareCSN;

                                SystemConfiguration sysConfig = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

                                if (sysConfig != null)
                                {


                                    foreach (CredentialFormat cardFormat in sysConfig.CredentialFormats)
                                    {
                                        if (cardFormat.Name == "CSN")
                                        {
                                            mifareCSN = cardFormat as CustomCredentialFormat;
                                            mifareCSN.SetValues(long.Parse(_NumeroCredencial));
                                            _credencial.Format = mifareCSN;
                                            break;
                                        }
                                        //
                                    }
                                }
                                break;
                        }

                        if (_credencial.Format != null)
                        {
                            _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

                            _cardholder.Credentials.Add(_credencial);
                        }
                        _credencialGuid = _credencial.Guid.ToString();
                        _sdk.TransactionManager.CommitTransaction();

                    }


                }
                catch (Exception ex)
                {
                    Utils.TraceException(ex);
                    return null;
                }

                return _cardholderGuid + "  " + _credencialGuid;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
                return null;
            }

        }
        public string CardHolder(string _Nome, string _CPF, string _CNPJ, string _Empresa, string _Matricula, string _Cargo,
                                            string _FC, string _NumeroCredencial, string _FormatoCredencial, string _DataValidadeSTR, string _LayoutCrachaGuid = "", Bitmap _Foto = null)
        {
            Cardholder _cardholder;
            string _firstname = "";
            string _lastname = "";
            string _cardholderGuid = "";
            try
            {
                _cardholder = BuscarCardHolder(_CPF, _CNPJ);
                string[] _nomeCompleto = _Nome.Split(' ');
                int _len = _nomeCompleto.Count();
                if (_len > 1)
                {
                    _lastname = _nomeCompleto[_len - 1];
                    _firstname = _nomeCompleto[0];
                }
                else
                {
                    _firstname = _Nome;
                }
                DateTime _DataValidade = DateTime.Now.AddSeconds(86399);

                if (_DataValidadeSTR != "")
                {
                    _DataValidade = Convert.ToDateTime(_DataValidadeSTR);
                    _DataValidade = _DataValidade.AddSeconds(86399);
                    _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
                }

                _sdk.TransactionManager.CreateTransaction();

                CardholderGroup _cardholderGroup;

                if (_cardholder == null)
                {
                    _cardholderGroup = _sdk.GetEntity(EntityType.CardholderGroup, 1) as CardholderGroup;
                    _cardholder = _sdk.CreateEntity(_Nome, EntityType.Cardholder) as Cardholder;
                    _cardholder.SetCustomFieldAsync("CPF", _CPF);
                    _cardholder.SetCustomFieldAsync("Empresa", _Empresa);
                    _cardholder.SetCustomFieldAsync("CNPJ", _CNPJ);
                    _cardholder.SetCustomFieldAsync("Matricula", _Matricula);
                    _cardholder.SetCustomFieldAsync("Cargo", _Cargo);
                    _cardholder.InsertIntoPartition(Partition.DefaultPartitionGuid);
                    if (_cardholderGroup != null)
                    {
                        _cardholder.Groups.Add(_cardholderGroup.Guid);
                    }
                }
                _cardholder.FirstName = _firstname;
                _cardholder.LastName = _lastname;
                _cardholder.SetCustomFieldAsync("CPF", _CPF);
                _cardholder.SetCustomFieldAsync("Empresa", _Empresa);
                _cardholder.SetCustomFieldAsync("CNPJ", _CNPJ);
                _cardholder.SetCustomFieldAsync("Matricula", _Matricula);
                _cardholder.SetCustomFieldAsync("Cargo", _Cargo);

                if (_Foto != null)
                {
                    _cardholder.Picture = _Foto;
                }

                _cardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);
                _cardholderGuid = _cardholder.Guid.ToString();

                _sdk.TransactionManager.CommitTransaction();

                return _cardholderGuid;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public string Credencial(string _LayoutCrachaGuid, string _FC, string _NumeroCredencial, string _FormatoCredencial, string cardholder)
        {
            string _credencialGuid = "";
            try
            {
                Cardholder _cardholder = _sdk.GetEntity(new Guid(cardholder)) as Cardholder;
                Credential _credencial;

                _credencial = BuscarCredencial(_FC, _NumeroCredencial);

                if (_credencial != null)
                {
                    if (_credencial.CardholderGuid != _cardholder.Guid)
                    {
                        //MessageBox.Show("Esta credencial pertence a outro usuário e não pode ser vinculada!", "Erro ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return null;
                    }
                    else // atualizar credencial
                    {
                        if (_LayoutCrachaGuid != "")
                        {
                            BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(_LayoutCrachaGuid)) as BadgeTemplate;
                            _credencial.BadgeTemplate = _BadgeTemplate.Guid;
                        }

                    }
                }
                else //criar nova credencial
                {
                    _sdk.TransactionManager.CreateTransaction();

                    _credencial = _sdk.CreateEntity(_NumeroCredencial, EntityType.Credential) as Credential;

                    _credencial.Name = _NumeroCredencial + " - " + _cardholder.FirstName + " " + _cardholder.LastName;

                    if (_LayoutCrachaGuid != "")
                    {
                        BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(_LayoutCrachaGuid)) as BadgeTemplate;
                        _credencial.BadgeTemplate = _BadgeTemplate.Guid;

                    }

                    switch (_FormatoCredencial)
                    {
                        case "Standard 26 bits":
                            _credencial.Format = new WiegandStandardCredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                            break;
                        case "H10302":
                            _credencial.Format = new WiegandH10302CredentialFormat(Convert.ToInt32(_NumeroCredencial));
                            break;
                        case "H10304":
                            _credencial.Format = new WiegandH10304CredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                            break;
                        case "H10306":
                            _credencial.Format = new WiegandH10306CredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                            break;
                        case "HID Corporate 1000":
                            _credencial.Format = new WiegandCorporate1000CredentialFormat(Convert.ToInt32(_FC), Convert.ToInt32(_NumeroCredencial));
                            break;

                        case "CSN":
                            CustomCredentialFormat mifareCSN;

                            SystemConfiguration sysConfig = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

                            if (sysConfig != null)
                            {


                                foreach (CredentialFormat cardFormat in sysConfig.CredentialFormats)
                                {
                                    if (cardFormat.Name == "CSN")
                                    {
                                        mifareCSN = cardFormat as CustomCredentialFormat;
                                        mifareCSN.SetValues(long.Parse(_NumeroCredencial));
                                        _credencial.Format = mifareCSN;
                                        break;
                                    }
                                    //
                                }
                            }
                            break;
                    }

                    if (_credencial.Format != null)
                    {
                        _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

                        _cardholder.Credentials.Add(_credencial);
                    }
                    _credencialGuid = _credencial.Guid.ToString();
                    _sdk.TransactionManager.CommitTransaction();

                }

                return _credencialGuid;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                return null;
            }
        }
        //public static bool VincularVeiculo(ClasseVeiculosCredenciais.VeiculoCredencial veiculoCredencial)
        //public static bool VincularVeiculo(string veiculoCredencial)
        //{
        //    try
        //    {
        //        ////IEngine _sdk = Main.engine;
        //        //Cardholder _cardholder;
        //        //string _firstname = "";
        //        //string _lastname = "";
        //        //////// CardHolder
        //        //try
        //        //{
        //        //    _sdk.TransactionManager.CreateTransaction();

        //        //    _cardholder = BuscarCardHolder(veiculoCredencial.Placa, veiculoCredencial.Cnpj);

        //        //    string[] _nomeCompleto = veiculoCredencial.VeiculoNome.Split(' ');

        //        //    int _len = _nomeCompleto.Count();

        //        //    if (_len > 1)
        //        //    {
        //        //        _lastname = _nomeCompleto[_len - 1];

        //        //        _firstname = _nomeCompleto[0];
        //        //    }
        //        //    else
        //        //    {
        //        //        _firstname = veiculoCredencial.VeiculoNome;
        //        //    }

        //        //    //DateTime _DataValidade;

        //        //    //if (vinculo.Validade != null)
        //        //    //{
        //        //    //    _DataValidade = (DateTime)vinculo.Validade;

        //        //    //    _DataValidade = _DataValidade.AddSeconds(86399);

        //        //    //    _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
        //        //    //}
        //        //    //else
        //        //    //{
        //        //    //    _DataValidade = DateTime.Now.AddSeconds(86399);
        //        //    //}


        //        //    CardholderGroup _cardholderGroup = _sdk.GetEntity(EntityType.CardholderGroup, 1) as CardholderGroup;

        //        //    if (_cardholder == null)
        //        //    {
        //        //        _cardholder = _sdk.CreateEntity(veiculoCredencial.VeiculoNome, EntityType.Cardholder) as Cardholder;
        //        //    }

        //        //    BitmapImage _img = Conversores.STRtoIMG(veiculoCredencial.VeiculoFoto) as BitmapImage;

        //        //    Bitmap _Foto = Conversores.BitmapImageToBitmap(_img);


        //        //    if (_Foto != null)
        //        //    {
        //        //        _cardholder.Picture = _Foto;
        //        //    }

        //        //    Bitmap _Motorista = null;


        //        //    _cardholder.SetCustomFieldAsync("No. do Veiculo", veiculoCredencial.VeiculoID);

        //        //    _cardholder.SetCustomFieldAsync("CPF", veiculoCredencial.Placa);

        //        //    _cardholder.SetCustomFieldAsync("Motorista", _img);

        //        //    _cardholder.SetCustomFieldAsync("Empresa", veiculoCredencial.EmpresaNome);

        //        //    _cardholder.SetCustomFieldAsync("Nome Fantasia", veiculoCredencial.EmpresaApelido);

        //        //    _cardholder.SetCustomFieldAsync("CNPJ", veiculoCredencial.CNPJ);

        //        //    _cardholder.SetCustomFieldAsync("Cargo", veiculoCredencial.Cargo);

        //        //    _cardholder.InsertIntoPartition(Partition.DefaultPartitionGuid);

        //        //    if (_cardholder.Groups.Count == 0 && _cardholderGroup != null)
        //        //    {
        //        //        _cardholder.Groups.Add(_cardholderGroup.Guid);
        //        //    }


        //        //    _cardholder.FirstName = _firstname;

        //        //    _cardholder.LastName = _lastname;


        //        //    //_cardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

        //        //    _sdk.TransactionManager.CommitTransaction();

        //        //    //veiculoCredencial.CardHolderGuid = _cardholder.Guid;
        //        //}
        //        //catch (Exception ex)
        //        //{

        //        //    return false;
        //        //}



        //        ////// Credencial
        //        /////
        //        //if (veiculoCredencial.FormatIDGUID != "00000000-0000-0000-0000-000000000000" && veiculoCredencial.NumeroCredencial != "")
        //        //{

        //        //    try
        //        //    {

        //        //        DateTime _DataValidade;

        //        //        if (veiculoCredencial.Validade != null)
        //        //        {
        //        //            _DataValidade = (DateTime)veiculoCredencial.Validade;

        //        //            _DataValidade = _DataValidade.AddSeconds(86399);

        //        //            _DataValidade = _DataValidade <= DateTime.Now ? DateTime.Now.AddSeconds(3) : _DataValidade;
        //        //        }
        //        //        else
        //        //        {
        //        //            _DataValidade = DateTime.Now.AddSeconds(86399);
        //        //        }

        //        //        Credential _credencial; // = _sdk.GetEntity((Guid)veiculoCredencial.CredencialGuid) as Credential;

        //        //        _credencial = BuscarCredencial(veiculoCredencial.NumeroCredencial, veiculoCredencial.FormatIDGUID, veiculoCredencial.FC);


        //        //        if (_credencial != null)
        //        //        {
        //        //            if (_credencial.CardholderGuid != _cardholder.Guid)
        //        //            {
        //        //                //MessageBox.Show("Esta credencial pertence a outro usuário e não pode ser vinculada!", "Erro ao Vincular", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        //        //                Global.PopupBox("Esta credencial já está associada a um usuário e não pode ser vinculada!", 4);

        //        //                return false;
        //        //            }

        //        //            if (veiculoCredencial.LayoutCrachaGUID != "")
        //        //            {
        //        //                BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(veiculoCredencial.LayoutCrachaGUID)) as BadgeTemplate;
        //        //                _credencial.BadgeTemplate = _BadgeTemplate.Guid;

        //        //                _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);
        //        //            }
        //        //        }
        //        //        else //criar nova credencial
        //        //        {


        //        //            _sdk.TransactionManager.CreateTransaction();

        //        //            _credencial = _sdk.CreateEntity("Credencial de " + _firstname, EntityType.Credential) as Credential;

        //        //            _credencial.Name = veiculoCredencial.NumeroCredencial + " - " + _firstname + " " + _lastname;

        //        //            _credencial.ActivationMode = new SpecificActivationPeriod(DateTime.Now, _DataValidade);

        //        //            if (veiculoCredencial.LayoutCrachaGUID != "")
        //        //            {
        //        //                BadgeTemplate _BadgeTemplate = _sdk.GetEntity(new Guid(veiculoCredencial.LayoutCrachaGUID)) as BadgeTemplate;
        //        //                _credencial.BadgeTemplate = _BadgeTemplate.Guid;

        //        //            }

        //        //            //0	N/D                           	00000000-0000-0000-0000-000000000000
        //        //            //1	Standard - 26 bits            	00000000-0000-0000-0000-000000000200
        //        //            //2	H10302 - 37 bits              	00000000-0000-0000-0000-000000000400
        //        //            //3	H10304 - 37 bits              	00000000-0000-0000-0000-000000000500
        //        //            //4	H10306 - 34 bits              	00000000-0000-0000-0000-000000000300
        //        //            //5	HID Corporate 1000 - 35 bits  	00000000-0000-0000-0000-000000000600
        //        //            //6	HID Corporate 1000 - 48 bits  	00000000-0000-0000-0000-000000000800
        //        //            //7	CSN                           	00000000-0000-0000-0000-000000000700


        //        //            switch (veiculoCredencial.FormatIDGUID)
        //        //            {
        //        //                case "00000000-0000-0000-0000-000000000200":
        //        //                    _credencial.Format = new WiegandStandardCredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
        //        //                    break;
        //        //                case "00000000-0000-0000-0000-000000000400":
        //        //                    _credencial.Format = new WiegandH10302CredentialFormat(Convert.ToInt32(veiculoCredencial.NumeroCredencial));
        //        //                    break;
        //        //                case "00000000-0000-0000-0000-000000000500":
        //        //                    _credencial.Format = new WiegandH10304CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
        //        //                    break;
        //        //                case "00000000-0000-0000-0000-000000000300":
        //        //                    _credencial.Format = new WiegandH10306CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
        //        //                    break;
        //        //                case "00000000-0000-0000-0000-000000000600":
        //        //                    _credencial.Format = new WiegandCorporate1000CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
        //        //                    break;
        //        //                case "00000000-0000-0000-0000-000000000800":
        //        //                    _credencial.Format = new Wiegand48BitCorporate1000CredentialFormat(Convert.ToInt32(veiculoCredencial.FC), Convert.ToInt32(veiculoCredencial.NumeroCredencial));
        //        //                    break;

        //        //                case "00000000-0000-0000-0000-000000000700":
        //        //                    CustomCredentialFormat mifareCSN;

        //        //                    SystemConfiguration sysConfig = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

        //        //                    if (sysConfig != null)
        //        //                    {


        //        //                        foreach (CredentialFormat cardFormat in sysConfig.CredentialFormats)
        //        //                        {
        //        //                            if (cardFormat.Name == "CSN")
        //        //                            {
        //        //                                mifareCSN = cardFormat as CustomCredentialFormat;
        //        //                                mifareCSN.SetValues(long.Parse(veiculoCredencial.NumeroCredencial));
        //        //                                _credencial.Format = mifareCSN;
        //        //                                break;
        //        //                            }
        //        //                            //
        //        //                        }
        //        //                    }
        //        //                    break;
        //        //            }

        //        //            //if (_credencial.Format != null)
        //        //            //{


        //        //            _credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

        //        //            _cardholder.Credentials.Add(_credencial);
        //        //            //}


        //        //            _sdk.TransactionManager.CommitTransaction();


        //        //        }

        //        //        //veiculoCredencial.CredencialGuid = _credencial.Guid;

        //            }
        //            catch (Exception ex)
        //            {

        //                return false;
        //            }
        //        }

        //        return true;
        //    }

        //    catch (Exception ex)
        //    {

        //        return false;
        //    }





        private static Cardholder BuscarCardHolder(string _CPF, string _CNPJ)
        {
            IEngine _sdk = Main.Engine;
            EntityConfigurationQuery query;

            QueryCompletedEventArgs result;

            try
            {
                query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

                query.EntityTypeFilter.Add(EntityType.Cardholder);

                query.NameSearchMode = StringSearchMode.StartsWith;

                result = query.Query();

                SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;

                var service = systemConfiguration.CustomFieldService;

                if (result.Success)
                {
                    foreach (DataRow dr in result.Data.Rows)
                    {
                        Cardholder _cardholder = _sdk.GetEntity((Guid)dr[0]) as Cardholder;

                        string _CPFteste = service.GetValue<string>("CPF", _cardholder.Guid);

                        string _CNPJteste = service.GetValue<string>("CNPJ", _cardholder.Guid);

                        if (_CPFteste == _CPF && _CNPJteste == _CNPJ)
                        {
                            return _cardholder;
                        }
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro na void BuscarCardHolder ex: " + ex);

                return null;
            }
        }

        //0	N/D                           	00000000-0000-0000-0000-000000000000
        //1	Standard - 26 bits            	00000000-0000-0000-0000-000000000200
        //2	H10302 - 37 bits              	00000000-0000-0000-0000-000000000400
        //3	H10304 - 37 bits              	00000000-0000-0000-0000-000000000500
        //4	H10306 - 34 bits              	00000000-0000-0000-0000-000000000300
        //5	HID Corporate 1000 - 35 bits  	00000000-0000-0000-0000-000000000600
        //6	HID Corporate 1000 - 48 bits  	00000000-0000-0000-0000-000000000800
        //7	CSN                           	00000000-0000-0000-0000-000000000700


        private static Credential BuscarCredencial(string _NumeroCredencial, string _FormatoCredencial, int _FC = 0)
        {
            IEngine _sdk = Main.Engine;
            EntityConfigurationQuery query;

            QueryCompletedEventArgs result;

            try
            {
                query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

                query.EntityTypeFilter.Add(EntityType.Credential);

                query.NameSearchMode = StringSearchMode.StartsWith;

                result = query.Query();

                if (result.Success)
                {
                    foreach (DataRow dr in result.Data.Rows)
                    {
                        Credential _credencial = _sdk.GetEntity((Guid)dr[0]) as Credential;

                        string _NumeroCredencialteste;
                        int _FCteste;
                        string _FormatoCredencialTeste = _credencial.Format.FormatId.ToString();
                        if (_FormatoCredencial == _FormatoCredencialTeste)
                        {

                            switch (_FormatoCredencialTeste)
                            {
                                case "00000000-0000-0000-0000-000000000200":
                                case "00000000-0000-0000-0000-000000000300":
                                case "00000000-0000-0000-0000-000000000500":
                                case "00000000-0000-0000-0000-000000000600":
                                case "00000000-0000-0000-0000-000000000800":
                                    _FCteste = ((WiegandStandardCredentialFormat)_credencial.Format).Facility;

                                    _NumeroCredencialteste = ((WiegandCredentialFormat)_credencial.Format).CardId.ToString();

                                    if (_FCteste == _FC && _NumeroCredencialteste == _NumeroCredencial)
                                    {
                                        return _credencial;
                                    }

                                    break;

                                case "00000000-0000-0000-0000-000000000400":
                                case "00000000-0000-0000-0000-000000000700":
                                    _NumeroCredencialteste = ((WiegandCredentialFormat)_credencial.Format).CardId.ToString();
                                    //_NumeroCredencialteste = long.Parse(_NumeroCredencialteste, System.Globalization.NumberStyles.HexNumber);

                                    if (_NumeroCredencialteste == _NumeroCredencial)
                                    {
                                        return _credencial;
                                    }
                                    break;
                            }

                        }

                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                 
                Utils.TraceException(ex);
                return null;
            }
        }

        //public static bool ExcluirCredencial(Guid? _CredencialGuid)
        //{
        //    try
        //    {

        //        if (_CredencialGuid == null || _CredencialGuid == new Guid("00000000-0000-0000-0000-000000000000"))
        //        {
        //            return true;
        //        }

        //        Credential _Credencial = _sdk.GetEntity((Guid)_CredencialGuid) as Credential;

        //        if (_Credencial != null)
        //        {
        //            _sdk.DeleteEntity(_Credencial);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }

        //    return true;
        //}


        #region Testes
        //public static void TesteCredencial()
        //{
        //    //IEngine _sdk = Main.engine;
        //    EntityConfigurationQuery query;
        //    QueryCompletedEventArgs result;
        //    try
        //    {
        //        //query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
        //        //query.EntityTypeFilter.Add(EntityType.Credential);

        //        //query.NameSearchMode = StringSearchMode.StartsWith;
        //        //result = query.Query();

        //        //if (result.Success)
        //        //{
        //        //    foreach (DataRow dr in result.Data.Rows)
        //        //    {
        //        //        Credential _credencial = _sdk.GetEntity((Guid)dr[0]) as Credential;

        //        //    }

        //        //}

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //Global.Log("Erro na void BuscarCardHolder ex: " + ex);

        //    //}
        //}
        #endregion

    }
}

