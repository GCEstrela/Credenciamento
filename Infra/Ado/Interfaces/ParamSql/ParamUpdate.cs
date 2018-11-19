// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Data;

#endregion

namespace Infra.Ado.Interfaces.ParamSql
{
    /// <summary>
    ///     Classe usada para receber e tratar parametros adequados a instruçao Update
    ///     <para>Independe do tipo de banco de dados, CrossDataBase</para>
    /// </summary>
    public class ParamUpdate : ParamInsert
    {
        #region Construtor

        public ParamUpdate(string campo, DbType tipo, object valor, bool key)
            : base(campo, tipo, valor, key)
        {
        }

        public ParamUpdate(string campo, object valor, bool key)
            : base(campo, valor, key)
        {
        }

        public ParamUpdate()
        {
        }

        #endregion
    }
}