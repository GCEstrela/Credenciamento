// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 11 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using Genetec.Sdk;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Activation;
using Genetec.Sdk.Entities.CustomEvents;
using Genetec.Sdk.Events;
using Genetec.Sdk.Queries;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Infra.Servicos.Entities;
using IMOD.Domain.Enums;
using System.Diagnostics;
using IMOD.Infra.Repositorios;
using System.Linq;

#endregion

namespace IMOD.Infra.Servicos
{
    public class CredencialGenetecService : ICredencialService
    {
        private readonly FormatoCredencialRepositorio _auxiliaresService = new FormatoCredencialRepositorio();

        private readonly IEngine _sdk;

        public CredencialGenetecService(IEngine sdk)
        {
            _sdk = sdk;

            //_sdk.EventReceived += new EventHandler<EventReceivedEventArgs>(_sdk_EventReceived);
            //_sdk.EntityInvalidated += new EventHandler<EntityInvalidatedEventArgs>(_sdk_EntityInvalidated);
            //_sdk.EntityRemoved += new EventHandler<EntityRemovedEventArgs>(_sdk_EntityRemoved);
            //EntityConfigurationQuery query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            ////query.EntityTypes.Add(EntityType.Alarm);
            //query.EntityTypes.Add(EntityType.Cardholder);
            //query.EntityTypes.Add(EntityType.Credential);
            ////query.Name = "a";
            //query.NameSearchMode = StringSearchMode.StartsWith;
            //query.QueryCompleted += new EventHandler<QueryCompletedEventArgs>(query_QueryCompleted);
            //query.BeginQuery();
        }

        #region  Metodos
        void query_QueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            if (e.Success)
            {
                foreach (DataRow dr in e.Data.Rows)
                {
                    Cardholder card = _sdk.GetEntity((Guid)dr[0]) as Cardholder;
                    if (card != null)
                    {

                    }
                    Credential cred = _sdk.GetEntity((Guid)dr[0]) as Credential;
                    if (cred != null)
                    {

                    }
                }
            }
            else
            {
                //_textboxConsole.Text += "The query has failed\r\n";
            }
        }
        private void _sdk_EventReceived(object sender, EventReceivedEventArgs e)
        {
            Entity entity = _sdk.GetEntity(e.SourceGuid);
            if (entity.EntityType == EntityType.Credential || entity.EntityType == EntityType.Cardholder)
            {
                var messa = e.Timestamp.ToString() + " - " + e.EventType.ToString() + " - " + entity.Name;
            }

            //if (entity != null)
            //{
            //    var messa = e.Timestamp.ToString() + " - " + e.EventType.ToString() + " - " + entity.Name;
            //}
        }
        public void _sdk_EntityRemoved(object sender, EntityRemovedEventArgs e)
        {

            Entity entity = _sdk.GetEntity(e.EntityGuid);
            //Credential _credential = _sdk.GetEntity(e.EntityGuid) as Credential;
            if (e.EntityType == EntityType.Credential)
            {
                Credential _credential = _sdk.GetEntity(e.EntityGuid) as Credential;


            }
            if (e.EntityType == EntityType.Cardholder)
            {
                Credential _credential = _sdk.GetEntity(e.EntityGuid) as Credential;
                // var state = _credential.State.ToString();
                //var data = _credential.ExpirationDate.ToString();
                //var messa = string.Format("{0}, {1} was modified {2}\r\n", entity.Name, state, data, e.IsLocalUpdate ? "locally" : "remotely");
                //var messa = string.Format("{0}, {1} was modified {2}\r\n", _credential.Name? "locally" : "remotely");
            }
            if (entity != null)
            {
                var messa = e.EntityGuid.ToString() + " - " + entity.Name;
            }
        }
        private void _sdk_EntityInvalidated(object sender, EntityInvalidatedEventArgs e)
        {
            Entity entity = _sdk.GetEntity(e.EntityGuid);
            if (entity != null)
            {
                if (entity.EntityType == EntityType.Credential)
                {
                    Credential _credential = _sdk.GetEntity(entity.Guid) as Credential;
                    var state = _credential.State.ToString();
                    var data = _credential.ExpirationDate.ToString();
                    var messa = string.Format("{0}, {1} was modified {2}\r\n", entity.Name, state, data, e.IsLocalUpdate ? "locally" : "remotely");



                }
                else
                {
                    Cardholder _cardholder = _sdk.GetEntity(entity.Guid) as Cardholder;
                    var state = _cardholder.State.ToString();
                    var data = _cardholder.ExpirationDate.ToString();
                    var messa = string.Format("{0}, {1} was modified {2}\r\n", entity.Name, state, data, e.IsLocalUpdate ? "locally" : "remotely");


                }
                //var ttt =entity.RunningState.ToString();

            }
        }
        private void ValidarCriarCardHolder(CardHolderEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //if (string.IsNullOrWhiteSpace(entity.Empresa)) throw new ArgumentNullException(nameof(entity.Empresa));
            //if (string.IsNullOrWhiteSpace(entity.Nome)) throw new ArgumentNullException(nameof(entity.Nome));
            //if (string.IsNullOrWhiteSpace(entity.Identificador)) throw new ArgumentNullException(nameof(entity.Identificador));
            if (string.IsNullOrWhiteSpace(entity.Matricula)) throw new ArgumentNullException(nameof(entity.Matricula));

        }

        private void SetValorCamposCustomizados(CardHolderEntity entity, Cardholder entityCardholder)
        {
            try
            {

                if (EncontrarCustonField("CPF"))
                {
                    entityCardholder.SetCustomFieldAsync("CPF", entity.Cpf);
                    entityCardholder.CustomFields["Cpf"] = entity.Cpf;
                }
                if (EncontrarCustonField("CNPJ"))
                {
                    entityCardholder.SetCustomFieldAsync("CNPJ", entity.Cnpj);
                    entityCardholder.CustomFields["Cnpj"] = entity.Cnpj;
                }
                if (EncontrarCustonField("Cargo"))
                {
                    entityCardholder.SetCustomFieldAsync("Cargo", entity.Cargo);
                    entityCardholder.CustomFields["Cargo"] = entity.Cargo;
                }
                if (EncontrarCustonField("Empresa"))
                {
                    entityCardholder.SetCustomFieldAsync("Empresa", entity.Empresa);
                    entityCardholder.CustomFields["Empresa"] = entity.Empresa;
                }
                if (EncontrarCustonField("Matricula"))
                {
                    entityCardholder.SetCustomFieldAsync("Matricula", entity.Matricula);
                    entityCardholder.CustomFields["Matricula"] = entity.Matricula;
                }
                if (EncontrarCustonField("Identificador"))
                {
                    entityCardholder.SetCustomFieldAsync("Identificador", entity.Identificador);
                    entityCardholder.CustomFields["Identificador"] = entity.Identificador;
                }
                if (!string.IsNullOrEmpty(entity.Nome))
                {
                    var primeironome = entity.Nome.Split(' ')[0];
                    if (primeironome != "")
                    {
                        entityCardholder.FirstName = primeironome;
                        entityCardholder.LastName = entity.Nome.Replace(primeironome, "").Trim();
                    }
                    else
                    {
                        entityCardholder.FirstName = entity.Nome;
                    }
                }
                entityCardholder.Description = entity.Empresa + " - " + entity.Cpf;

                //Uma data de validade deve ser mairo que a data corrente 
                //var compareData = DateTime.Compare(DateTime.Now, entity.Validade);
                //if (compareData >= 0) throw new InvalidOperationException("A data de validade deve ser maior que a data corrente.");

                entityCardholder.State = CardholderState.Active;
                if (entity.Validade > DateTime.Now)
                {
                    //if (entity.Ativo && entityCardholder.State == CardholderState.Active)
                    entityCardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, entity.Validade);
                }
                else
                {
                    entityCardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, entity.Validade.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                }

            }
            catch (Exception ex)
            {
                //ex = new Exception("");
                throw ex;
            }

        }

        private void SetValorFormatoCredencial(CardHolderEntity entity, Credential credencial)
        {
            try
            {
                int credecnailFormato = entity.FormatoCredencialId;
                switch (credecnailFormato)
                {
                    case (int)Tecnologia.Standard_26_bits:
                        credencial.Format = new WiegandStandardCredentialFormat(entity.FacilityCode, Convert.ToInt16(entity.NumeroCredencial));
                        break;
                    case (int)Tecnologia.HID_H10302_37_Bits:
                        credencial.Format = new WiegandH10302CredentialFormat(Convert.ToInt16(entity.NumeroCredencial));
                        break;
                    case (int)Tecnologia.HID_H10304_37_Bits:
                        credencial.Format = new WiegandH10304CredentialFormat(Convert.ToInt16(entity.FacilityCode), Convert.ToInt16(entity.NumeroCredencial));
                        break;
                    case (int)Tecnologia.HID_H10306_34_Bits:
                        credencial.Format = new WiegandH10306CredentialFormat(entity.FacilityCode, Convert.ToInt32(entity.NumeroCredencial));
                        break;
                    case (int)Tecnologia.HID_Corporate_1000_35_bits:
                        credencial.Format = new WiegandCorporate1000CredentialFormat(entity.FacilityCode, Convert.ToInt32(entity.NumeroCredencial));
                        break;
                    case (int)Tecnologia.HID_Corporate_1000_48_Bits:
                        credencial.Format = new WiegandCorporate1000CredentialFormat(entity.FacilityCode, Convert.ToInt32(entity.NumeroCredencial));
                        break;
                    case (int)Tecnologia.CSN:
                        credencial.Format = new WiegandCsn32CredentialFormat(long.Parse(entity.NumeroCredencial.ToString()));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                //throw ex;
            }
        }

        /// <summary>
        ///     Alterar uma credencial para um Card Holder (Titular do cartão)
        /// </summary>
        /// <param name="entity"></param>
        public void AlterarStatusCardHolder(CardHolderEntity entity)
        {
            try
            {

                bool ativo = false;
                if (string.IsNullOrWhiteSpace(entity.IdentificadorCardHolderGuid)) throw new ArgumentNullException(nameof(entity.IdentificadorCardHolderGuid));
                _sdk.TransactionManager.CreateTransaction();

                var cardholder = _sdk.GetEntity(new Guid(entity.IdentificadorCardHolderGuid)) as Cardholder;

                if (ativo)
                {
                    foreach (Guid element in cardholder.Credentials)
                    {
                        Credential credencialTMP = _sdk.GetEntity(new Guid(element.ToString())) as Credential;
                        var state = credencialTMP.State.ToString();
                        if (state == "Active")
                        {
                            ativo = true;
                            break;
                        }
                    }
                }

                if (entity.Validade > DateTime.Now)
                {
                    if (entity.Ativo)
                        cardholder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, entity.Validade);
                }

                if (cardholder.Credentials.Count == 0)
                {
                    cardholder.State = CardholderState.Inactive;
                }

                SetValorCamposCustomizados(entity, cardholder);


                if (!ativo)
                {
                    if (cardholder == null) throw new InvalidOperationException("Não foi possível encontrar o titular do cartão.");
                    cardholder.State = entity.Ativo ? CardholderState.Active : CardholderState.Inactive;
                }

                _sdk.TransactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Alterar uma credencial para um Card Holder (Titular do cartão)
        /// </summary>
        /// <param name="entity"></param>
        public void AlterarStatusCredencial(CardHolderEntity entity)
        {
            try
            {
                if (entity.IdentificadorCredencialGuid != null)
                {
                    if (string.IsNullOrWhiteSpace(entity.IdentificadorCredencialGuid)) throw new ArgumentNullException(nameof(entity.IdentificadorCredencialGuid));

                    var credencial = _sdk.GetEntity(new Guid(entity.IdentificadorCredencialGuid)) as Credential;
                    if (credencial == null) throw new InvalidOperationException("Não foi possível encontrar uma credencial.");
                    credencial.State = entity.Ativo ? CredentialState.Active : CredentialState.Inactive;
                    if (credencial.State != CredentialState.Active)
                    {
                        VerificaRegraAcesso(entity, false);
                    }
                    else
                    {
                        VerificaRegraAcesso(entity, true);
                    }
                }

            }
            catch (Exception ex)
            {
                _sdk.TransactionManager.RollbackTransaction();
                Utils.TraceException(ex);
                throw;
            }
        }
        public void AlterarStatusCredencial2(CardHolderEntity entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.IdentificadorCredencialGuid)) throw new ArgumentNullException(nameof(entity.IdentificadorCredencialGuid));

                var credencial = _sdk.GetEntity(new Guid(entity.IdentificadorCredencialGuid)) as Credential;
                if (credencial == null) throw new InvalidOperationException("Não foi possível encontrar uma credencial.");
                credencial.State = entity.Ativo ? CredentialState.Active : CredentialState.Inactive;

            }
            catch (Exception ex)
            {
                _sdk.TransactionManager.RollbackTransaction();
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     Criar Card Holder (Titular do cartão)
        ///     <para>Criar um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public void CriarCardHolder(CardHolderEntity entity)
        {

            //Validar dados
            ValidarCriarCardHolder(entity);
            //VerificaRegraAcesso(entity);
            try
            {

                Cardholder cardHolder = ObterCardHolder(entity);

                _sdk.TransactionManager.CreateTransaction();

                CardholderState Status = CardholderState.Active;
                if (cardHolder == null)
                {
                    cardHolder = _sdk.CreateEntity(entity.Nome, EntityType.Cardholder) as Cardholder;
                    cardHolder.State = CardholderState.Inactive;
                }
                Status = cardHolder.State;

                cardHolder.State = CardholderState.Active;
                cardHolder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, entity.Validade);
                cardHolder.State = Status;
                cardHolder.Picture = entity.Foto;

                if (entity.grupoAlterado)
                {
                    cardHolder.Groups.Clear();
                    if (entity.listadeGrupos != null)
                        foreach (Guid grupoGuid in entity.listadeGrupos)
                        {
                            cardHolder.Groups.Add(grupoGuid);
                        }
                }
                if (entity.GrupoPadrao != null && cardHolder.Groups.Count == 0)
                {
                    Guid grupo = new Guid(EncontrarGrupos(entity.GrupoPadrao));
                    if (grupo != null)
                        cardHolder.Groups.Add(grupo);
                }

                SetValorCamposCustomizados(entity, cardHolder);

                if (entity.regraAlterado)
                {
                    RemoverRegrasCardHolder(entity);

                    foreach (Guid regrasGuid in entity.listadeRegras)
                    {
                        AccessRule accesso_add = _sdk.GetEntity(regrasGuid) as AccessRule;
                        accesso_add.Members.Add(cardHolder.Guid);
                    }
                }

                if (!entity.Ativo)
                {
                    cardHolder.State = CardholderState.Inactive;
                }
                entity.IdentificadorCardHolderGuid = cardHolder.Guid.ToString();

                _sdk.TransactionManager.CommitTransaction();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        public Cardholder ObterCardHolder(CardHolderEntity entity)
        {
            try
            {
                Cardholder cardHolder;

                if (!string.IsNullOrWhiteSpace(entity.IdentificadorCardHolderGuid))
                {
                    cardHolder = _sdk.GetEntity(new Guid(entity.IdentificadorCardHolderGuid)) as Cardholder;
                    if (cardHolder == null)
                    {
                        cardHolder = EncontraCardHolderPelaMatricula(entity, entity.Matricula, true);
                    }
                    return cardHolder;
                }
                else
                {
                    cardHolder = EncontraCardHolderPelaMatricula(entity, entity.Matricula, true);
                    return cardHolder;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public void CriarCardHolder(CardHolderEntity entity, List<Guid> cardholderGuids)
        {

            //Validar dados
            ValidarCriarCardHolder(entity);
            //VerificaRegraAcesso(entity);
            try
            {
                #region Existindo CardHolder, não criar

                if (!string.IsNullOrWhiteSpace(entity.IdentificadorCardHolderGuid))
                {
                    var existEntity = _sdk.GetEntity(new Guid(entity.IdentificadorCardHolderGuid)) as Cardholder;
                    if (existEntity != null)
                    {
                        //Atualizar dados
                        SetValorCamposCustomizados(entity, existEntity);
                        //_sdk.TransactionManager.CommitTransaction();
                        //VerificaRegraAcesso(entity);
                        return;
                    }
                }
                else
                {
                    entity.IdentificadorCardHolderGuid = EncontraCardHolderPelaMatricula(entity, entity.Matricula);//Encontra Credencial pelo Numero da Matrícula
                }

                #endregion
                if (entity.IdentificadorCardHolderGuid == null)
                {
                    _sdk.TransactionManager.CreateTransaction();

                    var cardHolder = _sdk.CreateEntity(entity.Nome, EntityType.Cardholder) as Cardholder;
                    if (cardHolder == null) throw new InvalidOperationException("Não foi possível criar uma credencial.");

                    entity.IdentificadorCardHolderGuid = cardHolder.Guid.ToString(); //Set identificador Guid

                    SetValorCamposCustomizados(entity, cardHolder);

                    //EncontrarGrupos(entity.GrupoPadrao);
                    if (entity.GrupoPadrao != null)
                    {
                        try
                        {
                            //Guid grupo = new Guid(EncontrarGrupos(entity.GrupoPadrao));
                            //if (grupo != null)
                            //    cardHolder.Groups.Add(grupo);

                            foreach (Guid cardholderGuid in cardholderGuids)
                            {
                                Genetec.Sdk.Entities.CardholderGroup cardholdergroup = _sdk.GetEntity(cardholderGuid) as Genetec.Sdk.Entities.CardholderGroup;
                                cardHolder.Groups.Add(cardholdergroup.Guid);
                                //lista = cardholdergroup.Name + "\r\n" + lista;
                            }
                        }
                        catch (Exception)
                        {

                            //throw;
                        }

                    }
                    if (entity.Validade > DateTime.Now)
                    {
                        cardHolder.ActivationMode = new SpecificActivationPeriod(DateTime.Now, entity.Validade);
                    }
                    cardHolder.Picture = entity.Foto;


                    _sdk.TransactionManager.CommitTransaction();
                }


                //VerificaRegraAcesso(entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     Verifica Regras de Acesso 
        ///     <para>Add/Remove Regras de Acesso de um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public void VerificaRegraAcesso(CardHolderEntity entity, Boolean AddRemove = true)
        {

            if (!entity.Regras) return;

            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;

            try
            {
                var guid = new Guid(entity.IdentificadorCardHolderGuid);
                var cardHolder = _sdk.GetEntity(guid) as Cardholder;
                bool regraencontrada1 = false;
                bool regraencontrada2 = false;

                query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
                query.EntityTypeFilter.Add(EntityType.AccessRule);
                query.NameSearchMode = StringSearchMode.StartsWith;
                result = query.Query();
                SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                if (result.Success)
                {

                    foreach (DataRow dr in result.Data.Rows)    //sempre remove todas as regras de um CardHolder
                    {
                        AccessRule accesso_remover = _sdk.GetEntity((Guid)dr[0]) as AccessRule;
                        accesso_remover.Members.Remove(cardHolder.Guid);
                    }
                    AccessRule accesso;
                    foreach (DataRow dr in result.Data.Rows)
                    {
                        accesso = _sdk.GetEntity((Guid)dr[0]) as AccessRule;
                        var descricao = accesso.Name;
                        if (entity.Identificacao1 != null)
                        {
                            if (entity.Identificacao1.ToString() == descricao)
                            {
                                regraencontrada1 = true;
                                if (!AddRemove)
                                {
                                    accesso.Members.Remove(cardHolder.Guid);
                                }
                                else
                                {
                                    accesso.Members.Add(cardHolder.Guid);
                                }
                            }

                        }
                        if (entity.Identificacao2 != null)
                        {
                            if (entity.Identificacao2.ToString() == descricao)
                            {
                                regraencontrada2 = true;
                                if (!AddRemove)
                                {
                                    accesso.Members.Remove(cardHolder.Guid);
                                }
                                else
                                {
                                    accesso.Members.Add(cardHolder.Guid);
                                }
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        private void CriarRegra(string nomedaregra, Cardholder cardHolder)
        {
            try
            {

                var regra_1 = _sdk.CreateAccessRule(nomedaregra, AccessRuleType.Permanent);
                regra_1.Description = "Regra criada automáticamente pelo credenciamento";
                regra_1.Members.Add(cardHolder.Guid);

            }
            catch (Exception ex)
            {
                //throw ex;
            }


        }

        public void GerarEvento(string _evento, Entity _entidade = null, string _mensagem = "mensagem custom event")
        {
            try
            {
                CustomEventInstance _customEvent;
                if (_entidade == null)
                {
                    _customEvent = (CustomEventInstance)_sdk.ActionManager.BuildEvent(EventType.CustomEvent, Genetec.Sdk.SdkGuids.SystemConfiguration);
                }
                else
                {
                    _customEvent = (CustomEventInstance)_sdk.ActionManager.BuildEvent(EventType.CustomEvent, _entidade.Guid);
                }

                if (_customEvent != null)
                {
                    _customEvent.Id = new CustomEventId(Convert.ToInt32(_evento));
                    _customEvent.Message = _mensagem;

                    _sdk.ActionManager.RaiseEvent(_customEvent);

                }

            }
            catch (Exception)
            {

            }
        }
        public void DisparaAlarme(string menssagem, int IdAlarme)
        {
            try
            {

                _sdk.TransactionManager.CreateTransaction();

                TimeSpan duration = new TimeSpan(1, 12, 23, 62);
                var guidParent = new Guid("00000000-0000-0000-0000-000000000003"); //Guid do usuário que receberá os alarmes
                var guidAlarm = _sdk.CreateEntity(menssagem, EntityType.Alarm).Guid;
                Alarm alarm = (Alarm)_sdk.GetEntity(guidAlarm);
                alarm.Recipients.Add(guidParent, duration);


                _sdk.TransactionManager.CommitTransaction();

                _sdk.AlarmManager.TriggerAlarm(guidAlarm, _sdk.ClientGuid);

                //Alarm alarm = (Alarm)_sdk.GetEntity(EntityType.Alarm, IdAlarme);                
                //_sdk.AlarmManager.TriggerAlarm(alarm);

            }
            catch (Exception ex)
            {
                _sdk.TransactionManager.RollbackTransaction();
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     Remove Regras de Acesso 
        ///     <para>Remove Todas as Regras de Acesso de um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public string EncontraCredencialPeloNumero(CardHolderEntity entity, string credencialNumero)
        {

            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;
            try
            {
                var guid = new Guid(entity.IdentificadorCardHolderGuid);
                var credencial = _sdk.GetEntity(guid) as Credential;
                long decValue = 0;
                long credencialNum = 0;
                //int credecnailFormato = entity.FormatoCredencialId;


                query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
                query.EntityTypeFilter.Add(EntityType.Credential);
                query.NameSearchMode = StringSearchMode.StartsWith;

                result = query.Query();
                SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                if (result.Success)
                {

                    //_sdk.TransactionManager.CreateTransaction();
                    foreach (DataRow dr in result.Data.Rows)    //sempre remove todas as regras de um CardHolder
                    {

                        Credential cred = _sdk.GetEntity((Guid)dr[0]) as Credential;
                        var formatocredencialNm = cred.Format.Name;
                        var formatocredencialGuid = cred.Format.FormatId.ToString();
                        var formatocrdencial = _auxiliaresService.Listar(null, formatocredencialGuid).FirstOrDefault();
                        if (formatocrdencial != null)
                        {
                            //Debug.WriteLine(formatocredencialNm + " " + formatocredencialId);
                            var formatocredencialnumero = cred.Format.FormatId.ToString();
                            var credencialnumero = cred.Format.UniqueId;
                            
                            //long credencialValue =0;
                            switch (formatocrdencial.FormatoCredencialId)
                            {
                                case (int)Tecnologia.Standard_26_bits:

                                    credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    decValue = long.Parse(credencialnumero, System.Globalization.NumberStyles.HexNumber);
                                    credencialNum = Convert.ToInt64(entity.NumeroCredencial);
                                    if (credencialNum == decValue)
                                    {
                                        return cred.Guid.ToString();
                                    }


                                    //var numeroCredencial2 = cred.Name.Split('-');
                                    //string number2 = numeroCredencial2[0].ToString();
                                    //if (number2.Trim() == credencialNumero.Trim())
                                    //{
                                    //    return cred.Guid.ToString();
                                    //}
                                    break;
                                case (int)Tecnologia.HID_H10302_37_Bits:
                                    //credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    break;
                                case (int)Tecnologia.HID_H10304_37_Bits:
                                    //credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    break;
                                case (int)Tecnologia.HID_H10306_34_Bits:
                                    //credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    break;
                                case (int)Tecnologia.HID_Corporate_1000_35_bits:
                                    //credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    break;
                                case (int)Tecnologia.HID_Corporate_1000_48_Bits:
                                    //credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    break;
                                case (int)Tecnologia.CSN:
                                    credencialnumero = cred.Format.UniqueId.Split('|')[0];
                                    decValue = long.Parse(credencialnumero, System.Globalization.NumberStyles.HexNumber);
                                    credencialNum = Convert.ToInt64(entity.NumeroCredencial);                                    
                                    if (credencialNum == decValue)
                                    {
                                        return cred.Guid.ToString();
                                    }


                                    var numeroCredencial = cred.Name.Split('-');
                                    string number = numeroCredencial[0].ToString();
                                    if (number.Trim() == credencialNumero.Trim())
                                    {
                                        return cred.Guid.ToString();
                                    }
                                    break;
                                default:
                                    break;
                            }
                            //var credencialnumero = cred.Format.UniqueId.Split('|')[0];
                            //string decValue = (long.Parse(credencialnumero, System.Globalization.NumberStyles.HexNumber)).ToString();
                            try
                            {
                                //long decValue = long.Parse(credencialnumero, System.Globalization.NumberStyles.HexNumber);                                
                                //long credencialNum = Convert.ToInt64(entity.NumeroCredencial);
                                ////long myLong = long.Parse(entity.NumeroCredencial);
                                //if (credencialNum == credencialValue)
                                //{
                                //    return cred.Guid.ToString();
                                //}


                                //var numeroCredencial = cred.Name.Split('-');
                                //string number = numeroCredencial[0].ToString();
                                //if (number.Trim() == credencialNumero.Trim())
                                //{
                                //    return cred.Guid.ToString();
                                //}
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                        }

                    }
                    //_sdk.TransactionManager.CommitTransaction();
                }
                // entity.IdentificadorCredencialGuid = null;
                return "";
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     CardHolder
        ///     <para>Encontra um CardHolder se existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public string EncontraCardHolderPelaMatricula(CardHolderEntity entity, string cardholderMatricula)
        {
            if (!EncontrarCustonField("Matricula"))
            {
                throw new InvalidOperationException("Problemas para acessar o CustomFieds [ Matricula ]." + "\n" + "Entre em contato com o ADM do Genetec Security Center.");
            }

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
                        Cardholder cardholder = _sdk.GetEntity((Guid)dr[0]) as Cardholder;
                        if (cardholder.CustomFields["Matricula"].ToString() == cardholderMatricula.Trim())
                        {
                            return cardholder.Guid.ToString();
                        }
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        public Cardholder EncontraCardHolderPelaMatricula(CardHolderEntity entity, string cardholderMatricula, Boolean encontrado = true)
        {

            if (!EncontrarCustonField("Matricula"))
            {
                throw new InvalidOperationException("Problemas para acessar o CustomFieds [ Matricula ]." + "\n" + "Entre em contato com o ADM do Genetec Security Center.");
            }

            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;
            try
            {

                query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
                query.EntityTypeFilter.Add(EntityType.Cardholder);
                //query.EntityTypeFilter.Add(EntityType.CustomEntity);
                query.NameSearchMode = StringSearchMode.StartsWith;
                result = query.Query();
                SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                if (result.Success)
                {

                    foreach (DataRow dr in result.Data.Rows)
                    {
                        Cardholder cardholder = _sdk.GetEntity((Guid)dr[0]) as Cardholder;
                        if (cardholder.CustomFields["Matricula"].ToString() == cardholderMatricula.Trim())
                        {
                            return cardholder;
                        }
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        private QueryCompletedEventArgs GetEntities(string name, AccessRule eType)
        {
            var query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {

                query.Name = name;
                query.NameSearchMode = StringSearchMode.StartsWith;
                query.EntityTypeFilter.Add(EntityType.AccessRule);
                query.DownloadAllRelatedData = true;
                query.StrictResults = true;

                return query.Query();
            }
            return null;
        }
        /// <summary>
        ///     Criar Credencial para um Card Holder (Titular do cartão)
        ///     <para>Criar um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public void CriarCredencial(CardHolderEntity entity)
        {
            try
            {

                if (ExisteCardHolder(entity) == false)
                {
                    entity.IdentificadorCardHolderGuid = EncontraCardHolderPelaMatricula(entity, entity.Matricula);//Encontra Credencial pelo Numero da Matrícula
                    CriarCardHolder(entity);
                }

                VerificaRegraAcesso(entity, true);

                if (string.IsNullOrWhiteSpace(entity.IdentificadorCardHolderGuid)) throw new ArgumentNullException(nameof(entity.IdentificadorCardHolderGuid));
                if (string.IsNullOrWhiteSpace(entity.NumeroCredencial)) throw new ArgumentNullException(nameof(entity.NumeroCredencial));

                var guid = new Guid(entity.IdentificadorCardHolderGuid);
                var cardHolder = _sdk.GetEntity(guid) as Cardholder;
                if (cardHolder == null) throw new InvalidOperationException($"Não foi possível obter o titular do cartão GUID {entity.IdentificadorCardHolderGuid}");


                _sdk.TransactionManager.CreateTransaction();
                Credential credencial;

                #region Criar ou obter uma credencial

                if (entity.IdentificadorCredencialGuid == null)
                    entity.IdentificadorCredencialGuid = EncontraCredencialPeloNumero(entity, entity.NumeroCredencial);//Encontra Credencial pelo Numero da Credencial


                if (!string.IsNullOrWhiteSpace(entity.IdentificadorCredencialGuid))
                    credencial = _sdk.GetEntity(new Guid(entity.IdentificadorCredencialGuid)) as Credential;
                else
                    credencial = _sdk.CreateEntity(entity.NumeroCredencial, EntityType.Credential) as Credential;

                #endregion

                if (credencial != null)
                {
                    credencial.Name = $"{entity.NumeroCredencial} - {entity.Nome}";

                    SetValorFormatoCredencial(entity, credencial);

                    if (credencial.Format == null) throw new InvalidOperationException("Não foi possível criar credencial.");
                    credencial.InsertIntoPartition(Partition.DefaultPartitionGuid);

                    //Vincular Credencial ao CardHolder
                    if (credencial.Cardholder == null || credencial.Cardholder.Guid == cardHolder.Guid)
                    {
                        cardHolder.Credentials.Remove(credencial);
                        cardHolder.Credentials.Add(credencial);

                        if (cardHolder.State != CardholderState.Active) //Quando uma credencial é criada o cardholder fica sempre ativo.
                        {
                            cardHolder.State = CardholderState.Active;
                        }

                        entity.IdentificadorCredencialGuid = credencial.Guid.ToString();
                        entity.IdentificadorCredencialGuid = credencial.Guid.ToString();
                        if (entity.Foto != null)
                            cardHolder.Picture = entity.Foto;
                        SetValorCamposCustomizados(entity, cardHolder);

                        _sdk.TransactionManager.CommitTransaction();
                    }
                    else
                    {
                        string cardname = credencial.Cardholder.Name;
                        throw new InvalidOperationException("Credencial já esta associada a um CardHoldr:" + "\n" + cardname);
                    }
                }
                else
                {
                    entity.IdentificadorCredencialGuid = null;

                }
            }
            catch (Exception ex)
            {
                _sdk.TransactionManager.RollbackTransaction();
                Utils.TraceException(ex);
                throw ex;
            }
        }
        /// <summary>
        ///     Existe Card Holder (Titular do cartão)
        ///     <para>Verifica se CardHolder existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public bool ExisteCardHolder(CardHolderEntity entity)
        {
            try
            {
                #region Existindo CardHolder, não criar

                if (!string.IsNullOrWhiteSpace(entity.IdentificadorCardHolderGuid))
                {
                    var existEntity = _sdk.GetEntity(new Guid(entity.IdentificadorCardHolderGuid)) as Cardholder;
                    if (existEntity == null)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     Existe Card Holder (Titular do cartão)
        ///     <para>Verifica se CardHolder existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public bool ExisteCredential(CardHolderEntity entity)
        {
            try
            {
                #region Existindo Credential, não criar

                if (!string.IsNullOrWhiteSpace(entity.IdentificadorCredencialGuid))
                {
                    var existEntity = _sdk.GetEntity(new Guid(entity.IdentificadorCredencialGuid)) as Credential;
                    if (existEntity == null)
                    {
                        return false;
                    }
                }
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }


        /// <summary>
        ///     Verifica Regras de Acesso 
        ///     <para>Add/Remove Regras de Acesso de um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>        
        public void RemoverRegrasCardHolder(CardHolderEntity entity)
        {
            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;
            try
            {
                Cardholder cardHolder = ObterCardHolder(entity);
                //var guid = new Guid(entity.IdentificadorCardHolderGuid);
                //var cardHolder = _sdk.GetEntity(crdHolderGuid) as Cardholder;


                query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
                query.EntityTypeFilter.Add(EntityType.AccessRule);
                query.NameSearchMode = StringSearchMode.StartsWith;
                result = query.Query();
                SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                if (result.Success)
                {
                    //_sdk.TransactionManager.CreateTransaction();

                    foreach (DataRow dr in result.Data.Rows)    //sempre remove todas as regras de um CardHolder
                    {
                        AccessRule accesso = _sdk.GetEntity((Guid)dr[0]) as AccessRule;
                        accesso.Members.Remove(cardHolder.Guid);
                    }

                    //_sdk.TransactionManager.CommitTransaction();
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        public void DisparaAlarme(CardHolderEntity entity)
        {

        }
        public void RemoverCardHolder(CardHolderEntity entity)
        {
            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(entity.IdentificadorCardHolderGuid))
            //    {
            //        Cardholder credencial = _sdk.GetEntity(new Guid(entity.IdentificadorCardHolderGuid)) as Cardholder;
            //        if (credencial == null)
            //        {
            //            _sdk.TransactionManager.CreateTransaction();
            //            _sdk.DeleteEntity(credencial.Guid);
            //            _sdk.TransactionManager.CommitTransaction();
            //        }
            //        else
            //        {
            //            throw new InvalidOperationException("CardHolder não encontrado.");
            //        }

            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
        public void RemoverCredencial(CardHolderEntity entity)
        {
            try
            {
                #region Existindo Credential, deletar

                if (!string.IsNullOrWhiteSpace(entity.IdentificadorCredencialGuid))
                {

                    Credential credencial = _sdk.GetEntity(new Guid(entity.IdentificadorCredencialGuid)) as Credential;
                    //if (credencial != null)
                    if (credencial.Cardholder == null || credencial.Cardholder.Guid.ToString() == entity.IdentificadorCardHolderGuid)
                    {
                        _sdk.TransactionManager.CreateTransaction();

                        _sdk.DeleteEntity(credencial.Guid);

                        _sdk.TransactionManager.CommitTransaction();
                    }
                    else
                    {
                        string cardname = credencial.Cardholder.Name;
                        throw new InvalidOperationException("Credencial já esta associada a um CardHoldr: " + cardname);
                    }
                }


                #endregion
            }
            catch (Exception ex)
            {
                //_sdk.TransactionManager.RollbackTransaction;
                Utils.TraceException(ex);
                throw;
            }
        }
        public List<CardholderGroup> RetornarGrupos()
        {
            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;
            List<CardholderGroup> groupos = new List<CardholderGroup>();

            query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.CardholderGroup);
            query.NameSearchMode = StringSearchMode.StartsWith;
            result = query.Query();
            SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
            var service = systemConfiguration.CustomFieldService;
            if (result.Success)
            {
                foreach (DataRow dr in result.Data.Rows)    //sempre remove todas as regras de um CardHolder
                {
                    CardholderGroup grupocradholder = _sdk.GetEntity((Guid)dr[0]) as CardholderGroup;
                    groupos.Add(grupocradholder);
                }
            }
            return groupos;
        }
        public string EncontrarGrupos(string cardholdergrupo)
        {
            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;
            List<CardholderGroup> groupos = new List<CardholderGroup>();

            query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.CardholderGroup);
            query.NameSearchMode = StringSearchMode.StartsWith;
            result = query.Query();
            SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
            var service = systemConfiguration.CustomFieldService;
            if (result.Success)
            {
                foreach (DataRow dr in result.Data.Rows)    //sempre remove todas as regras de um CardHolder
                {
                    CardholderGroup grupocradholder = _sdk.GetEntity((Guid)dr[0]) as CardholderGroup;
                    if (grupocradholder.Name == cardholdergrupo.Trim())
                    {
                        return grupocradholder.Guid.ToString();
                    }
                }
            }
            return null;
        }
        /// <summary>
        ///     Verifica a existencia do CustonField 
        ///     Se existir insere o valor no campo.
        /// </summary>
        public Boolean EncontrarCustonField(string descricao)
        {
            var sysConfig = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
            if (sysConfig != null)
            {
                foreach (var campos in sysConfig.CustomFields)
                {
                    if (campos.Name == descricao)
                    {
                        return true;
                    }
                }
            }

            //sysConfig.CustomFields.Add"CAMPO-TESTE", EntityType.Cardholder, CustomFieldValueType.Text, string.Empty);
            return false;

        }

        public void RemoverCardHolder(ColaboradorEmpresa entity)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(entity.CardHolderGuid))
                {
                    Cardholder credencial = _sdk.GetEntity(new Guid(entity.CardHolderGuid)) as Cardholder;
                    if (credencial != null)
                    {
                        _sdk.TransactionManager.CreateTransaction();
                        _sdk.DeleteEntity(credencial.Guid);
                        _sdk.TransactionManager.CommitTransaction();
                    }
                    else
                    {
                        throw new InvalidOperationException("CardHolder não encontrado.");
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public  List<CardholderGroup> ObterGruposCardHolder(string cardHolder)
        //{
        //    try
        //    {
        //        Guid cardguid = new Guid(cardHolder);
        //        Cardholder cardholder = _sdk.GetEntity(cardguid) as Cardholder;
        //        return cardholder.Groups;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        #endregion
    }
}