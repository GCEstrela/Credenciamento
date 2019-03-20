// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 12 - 2019
// ***********************************************************************

#region

using IMOD.Infra.Servicos.Entities;

#endregion

namespace IMOD.Infra.Servicos
{
    public interface ICredencialService
    {
        #region  Metodos

        /// <summary>
        ///     Criar Card Holder (Titular do cartão)
        /// <para>Criar um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        void CriarCardHolder(CardHolderEntity entity);

        /// <summary>
        ///     Criar Credencial para um Card Holder (Titular do cartão)
        ///     <para>Criar um CardHolder se nao existir</para>
        /// </summary>
        /// <param name="entity"></param>
        void CriarCredencial(CardHolderEntity entity);

        /// <summary>
        ///     Alterar um status de um Card Holder (Titular do cartão)
        /// </summary>
        /// <param name="entity"></param>
        void AlterarStatusCardHolder(CardHolderEntity entity);

        /// <summary>
        ///     Alterar o status de uma credencial para um Card Holder (Titular do cartão)
        /// </summary>
        /// <param name="entity"></param>
        void AlterarStatusCredencial(CardHolderEntity entity);

        #endregion
    }
}