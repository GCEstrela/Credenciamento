// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IColaboradorRepositorio : IRepositorioBaseAdoNet<Colaborador>
    {
        #region  Metodos

        /// <summary>
        ///     Criar colaboradore e anexo
        /// </summary>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="anexos">Anexos</param>
        void CriarAnexos(Colaborador colaborador, IList<ColaboradorAnexo> anexos);

        /// <summary>
        ///     Listar Colaborador por status
        /// </summary>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        ICollection<Colaborador> ListarPorStatus(int idStatus);

        /// <summary>
        ///     Obter colaborador por CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        Colaborador ObterPorCpf(string cpf);

        /// <summary>
        ///     ListarPreCadastro
        /// </summary>
        /// <returns></returns>
        ICollection<Colaborador> ListarPreCadastro(params object[] objects);
        #endregion
    }
}