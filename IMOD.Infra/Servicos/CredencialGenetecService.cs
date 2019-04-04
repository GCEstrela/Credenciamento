// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 11 - 2019
// ***********************************************************************

#region

using System;
using System.Data;
using Genetec.Sdk;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Activation;
using Genetec.Sdk.Queries;
using IMOD.CrossCutting;
using IMOD.Infra.Servicos.Entities;

#endregion

namespace IMOD.Infra.Servicos
{
    public class CredencialGenetecService : ICredencialService
    {
        private readonly IEngine _sdk;

        public CredencialGenetecService(IEngine sdk)
        {
            _sdk = sdk;
        }

        #region  Metodos

        private void ValidarCriarCardHolder(CardHolderEntity entity)
        {
            if (entity == null) throw new ArgumentNullException (nameof(entity));
            if (string.IsNullOrWhiteSpace (entity.Empresa)) throw new ArgumentNullException (nameof(entity.Empresa));
            if (string.IsNullOrWhiteSpace (entity.Nome)) throw new ArgumentNullException (nameof(entity.Nome));
            if (string.IsNullOrWhiteSpace (entity.Identificador)) throw new ArgumentNullException (nameof(entity.Identificador));
            if (string.IsNullOrWhiteSpace (entity.Matricula)) throw new ArgumentNullException (nameof(entity.Matricula));
            
        }

        private void SetValorCamposCustomizados(CardHolderEntity entity, Cardholder entityCardholder)
        {
            entityCardholder.SetCustomFieldAsync ("CPF", entity.Cpf);
            entityCardholder.SetCustomFieldAsync ("CNPJ", entity.Cnpj);
            entityCardholder.SetCustomFieldAsync ("Cargo", entity.Cargo);
            entityCardholder.SetCustomFieldAsync ("Empresa", entity.Empresa);
            entityCardholder.SetCustomFieldAsync ("Matricula", entity.Matricula);
            entityCardholder.SetCustomFieldAsync ("Identificador", entity.Matricula);

            entityCardholder.FirstName = entity.Nome;
            entityCardholder.LastName = entity.Apelido;
            //Uma data de validade deve ser mairo que a data corrente 
            var compareData = DateTime.Compare (DateTime.Now, entity.Validade);
            if (compareData >= 0) throw new InvalidOperationException ("A data de validade deve ser maior que a data corrente.");
            entityCardholder.ActivationMode = new SpecificActivationPeriod (DateTime.Now, entity.Validade);
            entityCardholder.Picture = entity.Foto;
        }

        private void SetValorFormatoCredencial(CardHolderEntity entity, Credential credencial)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (entity.FormatoCredencial))
                    entity.FormatoCredencial = "CSN";

                switch (entity.FormatoCredencial.ToLower().Trim())
                {
                    case "standard 26 bits":
                        credencial.Format = new WiegandStandardCredentialFormat (entity.FacilityCode, Convert.ToInt16 (entity.NumeroCredencial));
                        break;
                    case "h10302":
                        credencial.Format = new WiegandH10302CredentialFormat (Convert.ToInt16 (entity.NumeroCredencial));
                        break;
                    case "h10304":
                        credencial.Format = new WiegandH10304CredentialFormat (Convert.ToInt16 (entity.FacilityCode), Convert.ToInt16 (entity.NumeroCredencial));
                        break;
                    case "h10306":
                        credencial.Format = new WiegandH10306CredentialFormat (entity.FacilityCode, Convert.ToInt32 (entity.NumeroCredencial));
                        break;
                    case "hid corporate 1000":
                        credencial.Format = new WiegandCorporate1000CredentialFormat (entity.FacilityCode, Convert.ToInt32 (entity.NumeroCredencial));
                        break;
                    default: //Format do tipo CSN

                        var sysConfig = _sdk.GetEntity (SdkGuids.SystemConfiguration) as SystemConfiguration;
                        CustomCredentialFormat mifareCsn;
                        if (sysConfig != null)
                            foreach (var cardFormat in sysConfig.CredentialFormats)
                                if (cardFormat.Name == "CSN")
                                {
                                    mifareCsn = cardFormat as CustomCredentialFormat;
                                    if (mifareCsn != null)
                                    {
                                        mifareCsn.SetValues (long.Parse (entity.NumeroCredencial));
                                        credencial.Format = mifareCsn;
                                    }

                                    break;
                                }

                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
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
                if (string.IsNullOrWhiteSpace (entity.IdentificadorCardHolderGuid)) throw new ArgumentNullException (nameof(entity.IdentificadorCardHolderGuid));
                _sdk.TransactionManager.CreateTransaction();

                var cardholder = _sdk.GetEntity (new Guid (entity.IdentificadorCardHolderGuid)) as Cardholder;
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
                
                if (!ativo)
                {
                    if (cardholder == null) throw new InvalidOperationException("Não foi possível encontrar o titular do cartão.");
                    cardholder.State = entity.Ativo ? CardholderState.Active : CardholderState.Inactive;
                }
                
                _sdk.TransactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
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
                if (string.IsNullOrWhiteSpace (entity.IdentificadorCredencialGuid)) throw new ArgumentNullException (nameof(entity.IdentificadorCredencialGuid));
               // _sdk.TransactionManager.CreateTransaction();

                var credencial = _sdk.GetEntity (new Guid (entity.IdentificadorCredencialGuid)) as Credential;
                if (credencial == null) throw new InvalidOperationException ("Não foi possível encontrar uma credencial.");
                credencial.State = entity.Ativo ? CredentialState.Active : CredentialState.Inactive;
                RemoveRegraAcesso(entity);  //emove todas as regras de aceso do cardholder
                if (credencial.State != CredentialState.Active)
                {
                    VerificaRegraAcesso(entity, false);
                }
                else
                {
                    VerificaRegraAcesso(entity, true);
                }
                //_sdk.TransactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                _sdk.TransactionManager.RollbackTransaction();
                Utils.TraceException (ex);
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
            ValidarCriarCardHolder (entity);
            //VerificaRegraAcesso(entity);
            try
            {
                _sdk.TransactionManager.CreateTransaction();

                #region Existindo CardHolder, não criar

                if (!string.IsNullOrWhiteSpace (entity.IdentificadorCardHolderGuid))
                {
                    var existEntity = _sdk.GetEntity(new Guid(entity.IdentificadorCardHolderGuid)) as Cardholder;
                    if (existEntity != null)
                    {
                        //Atualizar dados
                        SetValorCamposCustomizados(entity, existEntity);
                        _sdk.TransactionManager.CommitTransaction();
                        //VerificaRegraAcesso(entity);
                        return;
                    }
                }
                
                #endregion

                var cardHolder = _sdk.CreateEntity (entity.Nome, EntityType.Cardholder) as Cardholder;
                if (cardHolder == null) throw new InvalidOperationException ("Não foi possível criar uma credencial.");

                entity.IdentificadorCardHolderGuid = cardHolder.Guid.ToString(); //Set identificador Guid
                SetValorCamposCustomizados (entity, cardHolder);
                var cardHolderGroup = _sdk.GetEntity (EntityType.CardholderGroup, 1) as CardholderGroup;
                //if (cardHolderGroup == null) throw new InvalidOperationException ("Não foi possível gerar grupo de credencial");
                if (cardHolderGroup != null)
                    cardHolder.Groups.Add (cardHolderGroup.Guid);
               
                _sdk.TransactionManager.CommitTransaction();
                
                //VerificaRegraAcesso(entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
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

            //var accessRule = GetEntities(entity.Identificacao1.ToString(), EntityType.AccessRule);
            //if (accessRule != null)
           
            EntityConfigurationQuery query;
            QueryCompletedEventArgs result;
            try
            {
                bool regra1 = false;
                bool regra2 = false;
                var guid = new Guid(entity.IdentificadorCardHolderGuid);
                var cardHolder = _sdk.GetEntity(guid) as Cardholder;
               
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
                        AccessRule accesso = _sdk.GetEntity((Guid)dr[0]) as AccessRule;
                        accesso.Members.Remove(cardHolder.Guid);
                    }

                    foreach (DataRow dr in result.Data.Rows)
                    {

                        AccessRule accesso = _sdk.GetEntity((Guid)dr[0]) as AccessRule;
                        var descricao = accesso.Name;
                        if (entity.Identificacao1.ToString() == descricao)
                        {
                            if (!AddRemove)
                            {
                                accesso.Members.Remove(cardHolder.Guid);
                            }
                            else
                            {
                                accesso.Members.Add(cardHolder.Guid);
                            }
                            regra1 = true;
                        }
                        if (entity.Identificacao2.ToString() == descricao)
                        {
                            if (!AddRemove)
                            {
                                accesso.Members.Remove(cardHolder.Guid);
                            }
                            else
                            {
                                accesso.Members.Add(cardHolder.Guid);
                            }
                            regra2 = true;
                        }

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
        /// <summary>
        ///     Remove Regras de Acesso 
        ///     <para>Remove Todas as Regras de Acesso de um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveRegraAcesso(CardHolderEntity entity)
        {

            ////var accessRule = GetEntities(entity.Identificacao1.ToString(), EntityType.AccessRule);
            ////if (accessRule != null)

            //EntityConfigurationQuery query;
            //QueryCompletedEventArgs result;
            //try
            //{
            //    var guid = new Guid(entity.IdentificadorCardHolderGuid);
            //    var cardHolder = _sdk.GetEntity(guid) as Cardholder;

            //    query = _sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            //    query.EntityTypeFilter.Add(EntityType.AccessRule);
            //    query.NameSearchMode = StringSearchMode.StartsWith;
            //    result = query.Query();
            //    SystemConfiguration systemConfiguration = _sdk.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
            //    var service = systemConfiguration.CustomFieldService;
            //    if (result.Success)
            //    {
            //        _sdk.TransactionManager.CreateTransaction();
            //        foreach (DataRow dr in result.Data.Rows)    //sempre remove todas as regras de um CardHolder
            //        {
            //            AccessRule accesso = _sdk.GetEntity((Guid)dr[0]) as AccessRule;
            //            accesso.Members.Remove(cardHolder.Guid);
            //        }
            //        _sdk.TransactionManager.CommitTransaction();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //    throw;
            //}
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
                VerificaRegraAcesso(entity,true);
                if (string.IsNullOrWhiteSpace (entity.IdentificadorCardHolderGuid)) throw new ArgumentNullException (nameof(entity.IdentificadorCardHolderGuid));
                //if (string.IsNullOrWhiteSpace (entity.IdentificadorLayoutCrachaGuid)) throw new ArgumentNullException (nameof (entity.IdentificadorLayoutCrachaGuid));
                if (string.IsNullOrWhiteSpace (entity.NumeroCredencial)) throw new ArgumentNullException (nameof(entity.NumeroCredencial));

                var guid = new Guid (entity.IdentificadorCardHolderGuid);
                var cardHolder = _sdk.GetEntity (guid) as Cardholder;
                if (cardHolder == null) throw new InvalidOperationException ($"Não foi possível obter o titular do cartão GUID {entity.IdentificadorCardHolderGuid}");
                

                _sdk.TransactionManager.CreateTransaction();
                Credential credencial;
                
                #region Criar ou obter uma credencial

                if (!string.IsNullOrWhiteSpace (entity.IdentificadorCredencialGuid))
                    credencial = _sdk.GetEntity (new Guid (entity.IdentificadorCredencialGuid)) as Credential;
                else
                    credencial = _sdk.CreateEntity (entity.NumeroCredencial, EntityType.Credential) as Credential;

                #endregion

                if (credencial == null) throw new InvalidOperationException ("Não foi possível criar uma credencial.");
                credencial.Name = $"{entity.NumeroCredencial} - {entity.Nome}";
                //var layout = _sdk.GetEntity (new Guid (entity.IdentificadorLayoutCrachaGuid));
                //if (layout != null) //Especifica um layout Cracha apenas se houver um existente
                //    credencial.BadgeTemplate = new Guid (entity.IdentificadorLayoutCrachaGuid);
                //Obter Formatacao da Credencial
                SetValorFormatoCredencial (entity, credencial);

                if (credencial.Format == null) throw new InvalidOperationException ("Não foi possível criar credencial.");
                credencial.InsertIntoPartition (Partition.DefaultPartitionGuid);

                //Vincular Credencial ao CardHolder
                cardHolder.Credentials.Add (credencial);
                if (cardHolder.State != CardholderState.Active) //Quando uma creencial é criada o cardholder fica semtre ativo.
                {
                    cardHolder.State = CardholderState.Active;
                } 
                entity.IdentificadorCredencialGuid = credencial.Guid.ToString();

                _sdk.TransactionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                throw;
            }
        }

        #endregion
    }
}