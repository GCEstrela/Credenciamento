// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Data;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Ado.Interfaces
{
    /// <summary>
    ///     Tipo de Base de Dados
    /// </summary>
    public enum TipoDataBase
    {
        //PostgreSql,
        SqlServer
        // Oracle
    }

    public enum TipoInstrucao
    {
        InsertText,
        DeleteText,
        SelectText,
        UpdateText
    }

    public interface IDataBaseAdo
    {
        /// <summary>
        ///     String de conexão
        /// </summary>
        string Connectionstring { get; set; }

        /// <summary>
        ///     Nome da instrução SQl executada no momento
        ///     <para>Para instruções InsertText,DeleteText,UpdateText,SelectText </para>
        /// </summary>
        TipoInstrucao SqlText { get; }

        /// <summary>
        ///     Cria uma conexção com o banco de dados
        /// </summary>
        /// <returns>Um objeto Connection</returns>
        IDbConnection CreateConnection();

        /// <summary>
        ///     Cria um objeto Command
        /// </summary>
        /// <returns>Um objeto command</returns>
        IDbCommand CreateCommand();

        /// <summary>
        ///     Abre conexão com o banco de dados
        /// </summary>
        /// <returns>Um objeto Connection</returns>
        IDbConnection CreateOpenConnection();

        /// <summary>
        ///     Abre conexão com o banco de dados
        /// </summary>
        /// <param name="connectionstring">String de conexão</param>
        /// <returns>Um objeto Connection</returns>
        IDbConnection CreateOpenConnection(string connectionstring);

        /// <summary>
        ///     Usada para execução de instrução SQL in line
        /// </summary>
        /// <param name="commandText">Uma instrução SQL</param>
        /// <param name="connection">Conexão com o banco de dados</param>
        /// <returns>Um objeto command</returns>
        IDbCommand CreateCommand(string commandText, IDbConnection connection);

        /// <summary>
        ///     Usada para execução de instrução SQL Stored de Procedure
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <param name="connection">Conexão com o banco de dados</param>
        /// <returns>Um objeto Command</returns>
        IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);

        /// <summary>
        ///     Criar parametro de entrada ou saida para a insrução
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro</param>
        /// <param name="parameterType">Tipo do parâmetro</param>
        /// <param name="direction">Direção do parâmetro</param>
        /// <param name="value">Valor do parâmetro</param>
        /// <param name="size">Tamanho do parâmetro</param>
        /// <param name="sourceCollumn">Fonte da coluna</param>
        /// <param name="isNullable">Nulidade do parâmetro</param>
        /// <param name="precision">Precisão</param>
        /// <param name="scale">Escala</param>
        /// <param name="sourceVersion">Versão</param>
        /// <returns>Um objeto DataParameter </returns>
        IDbDataParameter CreateParameter(string parameterName, DbType parameterType, ParameterDirection direction,
            object value, int size = 0, string sourceCollumn = "", bool isNullable = false, byte precision = 0,
            byte scale = 0, DataRowVersion sourceVersion = DataRowVersion.Current);

        /// <summary>
        ///     Cria um objeto DataAdpter
        /// </summary>
        /// <returns>Um objeto DataAdpter</returns>
        IDbDataAdapter CreateDataAdapter();

        /// <summary>
        ///     Monta sintaxe SQL Insert
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        IDbCommand InsertText(string tabela, IDbConnection connection);


        IDbCommand InsertTextSemRetorno(string tabela, IDbConnection connection);

        /// <summary>
        ///     Monta sintaxe SQL Delete
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        IDbCommand DeleteText(string tabela, IDbConnection connection);

        /// <summary>
        ///     Monta sintaxe SQL Select
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        IDbCommand SelectText(string tabela, IDbConnection connection);
        IDbCommand SelectSQL(string sqlQuery, IDbConnection connection);
        /// <summary>
        ///     Monta sintaxe SQL Update
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        IDbCommand UpdateText(string tabela, IDbConnection connection);

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro insert</param>
        IDbDataParameter CreateParameter(ParamDelete oParam);

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro Select</param>
        IDbDataParameter CreateParameter(ParamSelect oParam);

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro insert</param>
        IDbDataParameter CreateParameter(ParamInsert oParam);

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro Update</param>
        IDbDataParameter CreateParameter(ParamUpdate oParam);

        /// <summary>
        ///     Informações do banco de dados
        /// </summary>
        /// <returns></returns>
        DataBaseInfo Info();

        /// <summary>
        ///     Informações do banco de dados
        /// </summary>
        /// <param name="connectionstring">Uma string de conexao</param>
        /// <returns></returns>
        DataBaseInfo Info(string connectionstring);
    }
}