// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 12 - 2019
// ***********************************************************************

#region

using Genetec.Sdk.Entities;
using IMOD.Infra.Servicos.Entities;

#endregion

namespace IMOD.Infra.Servicos
{
    public interface ICredencialService
    {
        #region  Metodos

        /// <summary>
        ///     Criar Card Holder (Titular do cart�o)
        /// <para>Criar um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        void CriarCardHolder(CardHolderEntity entity);

        /// <summary>
        ///     Criar Credencial para um Card Holder (Titular do cart�o)
        ///     <para>Criar um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        void CriarCredencial(CardHolderEntity entity);

        /// <summary>
        ///     Alterar um status de um Card Holder (Titular do cart�o)
        /// </summary>
        /// <param name="entity"></param>
        void AlterarStatusCardHolder(CardHolderEntity entity);

        /// <summary>
        ///     Alterar o status de uma credencial para um Card Holder (Titular do cart�o)
        /// </summary>
        /// <param name="entity"></param>
        void AlterarStatusCredencial(CardHolderEntity entity);
        /// <summary>
        ///     Verifica se existe Card Holder (Titular do cart�o)
        /// </summary>
        /// <param name="entity"></param>
        bool ExisteCardHolder(CardHolderEntity entity);
        /// <summary>
        ///     Verifica se existe Credencial (Titular do cart�o)
        /// </summary>
        /// <param name="entity"></param>
        bool ExisteCredential(CardHolderEntity entity);
        /// <summary>
        ///     Remover Regras de acesso de um Card Holder (Titular do cart�o)
        /// </summary>
        /// <param name="entity"></param>
        void RemoverRegrasCardHolder(CardHolderEntity entity);
        void RemoverCredencial(CardHolderEntity entity);
        void DisparaAlarme(string menssagem, int IdAlarme);
        void GerarEvento(string _evento, Entity _entidade = null, string _mensagem = "mensagem custom event");
        #endregion
    }
}