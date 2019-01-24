// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using IMOD.CrossCutting;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Ado.SQLServer
{
    public class SqlServerDataBase : IDataBaseAdo
    {
        /// <summary>
        ///     Cria uma conexção com o banco de dados
        /// </summary>
        /// <returns>Um objeto Connection</returns>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(Connectionstring);
        }

        /// <summary>
        ///     Cria um objeto Command
        /// </summary>
        /// <returns>Um objeto command</returns>
        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        ///     Abre conexão com o banco de dados
        /// </summary>
        /// <returns>Um objeto Connection</returns>
        public IDbConnection CreateOpenConnection()
        {
            try
            {
                var connection = (SqlConnection) CreateConnection();
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                Utils.TraceException(ex);
                throw  new Exception($"Ocorreu uma falha ao conectar com o banco de dados\nRazão:\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                return null;
            }
        }

        /// <summary>
        ///     Abre conexão com o banco de dados
        /// </summary>
        /// <param name="connectionstring">String de conexão</param>
        /// <returns>Um objeto Connection</returns>
        public IDbConnection CreateOpenConnection(string connectionstring)
        {
            try
            {
                var connection = new SqlConnection(connectionstring);
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                Utils.TraceException(ex);
                throw new Exception($"Ocorreu uma falha ao conectar com o banco de dados\nRazão:\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                return null;
            }
          
        }

        /// <summary>
        ///     Usada para execução de instrução SQL in line
        /// </summary>
        /// <param name="commandText">Uma instrução SQL</param>
        /// <param name="connection">Conexão com o banco de dados</param>
        /// <returns>Um objeto command</returns>
        public IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            var command = (SqlCommand) CreateCommand();
            command.CommandText = commandText;
            command.Connection = (SqlConnection) connection;
            command.CommandType = CommandType.Text;
            //Set command
            _dbCommand = command;
            return command;
        }

        /// <summary>
        ///     Usada para execução de instrução SQL Stored de Procedure
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <param name="connection">Conexão com o banco de dados</param>
        /// <returns>Um objeto Command</returns>
        public IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            var command = (SqlCommand) CreateCommand();
            command.CommandText = procName;
            command.Connection = (SqlConnection) connection;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

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
        public IDbDataParameter CreateParameter(string parameterName, DbType parameterType,
            ParameterDirection direction,
            object value, int size = 0, string sourceCollumn = "", bool isNullable = false, byte precision = 0,
            byte scale = 0, DataRowVersion sourceVersion = DataRowVersion.Current)
        {
            var tipo = DbTypeParaSqlDbTypes[parameterType];
            return new SqlParameter(parameterName, tipo, size, direction, isNullable, precision, scale, sourceCollumn,
                sourceVersion, value);
        }

        /// <summary>
        ///     Cria um objeto DataAdpter
        /// </summary>
        /// <returns>Um objeto DataAdpter</returns>
        public IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        /// <summary>
        ///     Monta sintaxe SQL Insert
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        public IDbCommand InsertText(string tabela, IDbConnection connection)
        {
            //Reset parametros
            Clear();
            SqlText = TipoInstrucao.InsertText;
            //Set parametros 
            _tabelaNome = tabela;
            _sintaxeSql = "insert into {0} ({1}) values ({2});select scope_identity();";
            _dbCommand = CreateCommand();
            _dbCommand.Connection = connection; //atribuição objeto de conexao ao objeto command

            var sql = string.Format(_sintaxeSql, tabela.Trim(), "", "", "0").ToLower();
            _dbCommand.CommandText = sql.Trim();
            _dbCommand.CommandType = CommandType.Text;
            return _dbCommand;
        }

        /// <summary>
        ///     Monta sintaxe SQL Delete
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        public IDbCommand DeleteText(string tabela, IDbConnection connection)
        {
            //Reset parametros
            Clear();
            SqlText = TipoInstrucao.DeleteText;
            //Set parametros 
            _tabelaNome = tabela;
            _sintaxeSql = "delete from {0} {1};"; //sintaxe sql
            _dbCommand = CreateCommand();
            _dbCommand.Connection = connection; //atribuição objeto de conexao ao objeto command

            var sql = string.Format(_sintaxeSql, tabela.Trim(), "").ToLower();
            _dbCommand.CommandText = sql.Trim();
            _dbCommand.CommandType = CommandType.Text;
            return _dbCommand;
        }

        /// <summary>
        ///     Monta sintaxe SQL Select
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        public IDbCommand SelectText(string tabela, IDbConnection connection)
        {
            //Reset parametros
            Clear();
            SqlText = TipoInstrucao.SelectText;
            //Set parametros
            _tabelaNome = tabela;
            _sintaxeSql = "select * from {0} {1} {2}"; //sintaxe sql
            _dbCommand = CreateCommand();
            _dbCommand.Connection = connection; //atribuição objeto de conexao ao objeto command

            var sql = string.Format(_sintaxeSql, tabela.Trim(), "", "").ToLower();
            _dbCommand.CommandText = sql.Trim() + ";";
            _dbCommand.CommandType = CommandType.Text;
            return _dbCommand;
        }

        /// <summary>
        ///     Monta sintaxe SQL Update
        /// </summary>
        /// <param name="tabela">Nome da tabela </param>
        /// <param name="connection">Um objeto connection</param>
        /// <returns></returns>
        public IDbCommand UpdateText(string tabela, IDbConnection connection)
        {
            //Reset parametros
            Clear();
            SqlText = TipoInstrucao.UpdateText;
            _whereUpdate = "";
            //Set parametros
            _tabelaNome = tabela;
            _sintaxeSql = "update {0} set {1} {2};";
            _dbCommand = CreateCommand();
            _dbCommand.Connection = connection; //atribuição objeto de conexao ao objeto command

            var sql = string.Format(_sintaxeSql, tabela.Trim(), "", "").ToLower();
            _dbCommand.CommandText = sql.Trim() + ";";
            _dbCommand.CommandType = CommandType.Text;
            return _dbCommand;
        }

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro insert</param>
        public IDbDataParameter CreateParameter(ParamDelete oParam)
        {
            _pksDelete.Add(oParam.Concatene);
            var keys = JoinString(" AND ", _pksDelete); //Adiciona o operador And a sintaxe
            //Se houver equals informados no parametro, acrescente a clausula where
            var where = string.IsNullOrWhiteSpace(keys) ? string.Empty : "where" + keys;
            //Processar string SQL
            var sql = string.Format(_sintaxeSql, _tabelaNome.Trim(), where.Trim()).ToLower();
            //Associar string SQL
            _dbCommand.CommandText = sql.Trim();
            _dbCommand.CommandType = CommandType.Text;

            //Adiciona parametro
            var parameterName = "@" + oParam.Campo;
            var dbType = oParam.Tipo;
            var parameterDirection = ParameterDirection.Input;
            var value = oParam.Valor;
            return CreateParameter(parameterName, dbType, parameterDirection, value);
        }

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro Select</param>
        public IDbDataParameter CreateParameter(ParamSelect oParam)
        {
            //Se não houver valor informado nao criar parametro de entrada, nem clausula where na sintaxe
            if (string.IsNullOrWhiteSpace(oParam.Valor?.ToString())) return null;
            //Descartar valor nulo
            if (oParam.Valor != null)
            {
                //Descarta objetos nao nulos, mas vazio, ou seja ""
                if (IsEmptyOrZero(oParam.Valor) == false) _parameters.Add(oParam.Concatene);

                //Verificar campos ordenados
                if (oParam.Order != null)
                {
                    var sort = oParam.Order == ParamSelect.OrderBy.Desc ? "DESC" : string.Empty;
                    //Campo, ASC
                    var format = string.Format("{0} {1}", oParam.Campo, sort);
                    _orderby.Add(format.Trim());
                }
            }

            //Adiciona o operador And a sintaxe
            var parameters = JoinString(" AND ", _parameters);
            //Preparar clasula OrderBy se houver
            var orderby = _orderby.Count > 0 ? "order by " + JoinString(",", _orderby) : string.Empty;
            //Se houver equals informados no parametro, acrescente a clausula where
            var where = string.IsNullOrWhiteSpace(parameters) ? string.Empty : "where" + parameters;
            //Processar string SQL
            var sql = string.Format(_sintaxeSql, _tabelaNome.Trim(), where.Trim(), orderby.Trim()).ToLower();
            //Associar string SQL
            _dbCommand.CommandText = sql.Trim() + ";";
            _dbCommand.CommandType = CommandType.Text;

            //Adiciona parametro
            var parameterName = "@" + oParam.Campo;
            var dbType = oParam.Tipo;
            var parameterDirection = ParameterDirection.Input;
            var value = oParam.Valor;
            return CreateParameter(parameterName, dbType, parameterDirection, value);
        }

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro insert</param>
        public IDbDataParameter CreateParameter(ParamInsert oParam)
        {
            //concatena campos nao primary key
            if (oParam.Key == false)
            {
                _valorInsert.Add("@" + oParam.Campo);
                _campoInsert.Add(oParam.Campo);
            }
            else
            {
                //concatena campos primary key para retorno do numero sequencial gerado no momento da insercao
                _pksInsert.Add(oParam.Campo);
            }


            var values = JoinString(",", _valorInsert); //Adiciona virgula a sintaxe
            var campo = JoinString(",", _campoInsert); //Adiciona virgula a sintaxe

            var sql = string.Format(_sintaxeSql.Trim(), _tabelaNome.Trim(), campo.Trim(), values.Trim()).ToLower();

            //Associar string SQL
            _dbCommand.CommandText = sql.Trim();
            _dbCommand.CommandType = CommandType.Text;
            //Adiciona parametro
            var parameterName = "@" + oParam.Campo;
            var dbType = oParam.Tipo;
            var parameterDirection = ParameterDirection.Input;
            var value = oParam.Valor;
            return CreateParameter(parameterName, dbType, parameterDirection, value);
        }

        /// <summary>
        ///     Cria parametro de entrada da instrução
        /// </summary>
        /// <param name="oParam">Parametro Update</param>
        public IDbDataParameter CreateParameter(ParamUpdate oParam)
        {
            //concatena campos nao primary key
            if (oParam.Key == false)
            {
                //concatena camposde uma instrução Update ex: campo=@campo
                var campoSet = oParam.Campo + "=" + "@" + oParam.Campo;
                //Adiciona a lista de campo
                _campoUpdate.Add(campoSet);
            }
            else
            {
                //concatena campos primary key ex: campo=@campo
                var campoWhere = oParam.Campo + "=" + "@" + oParam.Campo;
                //Adiciona a lista de campos
                _pksUpdate.Add(campoWhere);
                _whereUpdate = "where ";
            }

            var campo = JoinString(",", _campoUpdate);
            var pks = JoinString(" AND ", _pksUpdate);

            //Processar string SQL
            var sql = string.Format(_sintaxeSql, _tabelaNome.Trim(), campo.Trim(), _whereUpdate + pks).ToLower();

            //Associar string SQL
            _dbCommand.CommandText = sql.Trim();
            _dbCommand.CommandType = CommandType.Text;

            //Adiciona parametro
            var parameterName = "@" + oParam.Campo;
            var dbType = oParam.Tipo;
            var parameterDirection = ParameterDirection.Input;
            var value = oParam.Valor;
            return CreateParameter(parameterName, dbType, parameterDirection, value);
        }

        /// <summary>
        ///     Informações do banco de dados
        /// </summary>
        /// <returns></returns>
        public DataBaseInfo Info()
        {
            return Info(Connectionstring);
        }

        /// <summary>
        ///     Informações do banco de dados
        /// </summary>
        /// <param name="connectionstring">Uma string de conexao</param>
        /// <returns></returns>
        public DataBaseInfo Info(string connectionstring)
        {
            var dados = new DataBaseInfo();
            dados.ConnexaoEstabelecida = false;
            dados.ProviderName = "SqlServer";

            using (var conn = new SqlConnection(connectionstring))
            {
                //Abrir conexao
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    dados.Host = conn.WorkstationId;
                    dados.ConnexaoEstabelecida = true;
                    dados.VersaoServidor = conn.ServerVersion;
                    dados.VersaoProvider = conn.ServerVersion;
                    dados.BaseDados = conn.Database;
                    dados.StringConexao = conn.ConnectionString;
                }

                conn.Close();
            }

            return dados;
        }

        #region Propriedades

        private readonly List<string> _campoInsert = new List<string>(); //armazena um array de campos do insert
        private readonly List<string> _campoUpdate = new List<string>(); //armazena um array de campos do update
        private readonly List<string> _orderby = new List<string>(); //armazenas campos order by
        private readonly List<string> _parameters = new List<string>(); //armazena parametros
        private readonly List<string> _pksDelete = new List<string>(); //armazena um array de campos chaves do insert
        private readonly List<string> _pksInsert = new List<string>(); //armazena um array de campos chaves do insert
        private readonly List<string> _pksUpdate = new List<string>(); //armazena um array de campos chaves do update
        private readonly List<string> _valorInsert = new List<string>(); //armazena um array de campos do insert

        private IDbCommand _dbCommand;
        private string _sintaxeSql;
        private string _tabelaNome;
        private string _whereUpdate;


        /// <summary>
        ///     String de conexão
        /// </summary>
        public string Connectionstring { get; set; }

        /// <summary>
        ///     Nome da instrução SQl executada no momento
        ///     <para>Para instruções InsertText,DeleteText,UpdateText,SelectText </para>
        /// </summary>
        public TipoInstrucao SqlText { get; private set; }

        private static readonly Dictionary<DbType, SqlDbType> DbTypeParaSqlDbTypes = new Dictionary<DbType, SqlDbType>
        {
            {DbType.Byte, SqlDbType.Binary},
            {DbType.DateTime2, SqlDbType.DateTime2},
            {DbType.Guid, SqlDbType.VarChar},
            {DbType.Currency, SqlDbType.Money},
            {DbType.Int16, SqlDbType.SmallInt},
            {DbType.Int32, SqlDbType.Int},
            {DbType.Int64, SqlDbType.BigInt},
            {DbType.UInt64, SqlDbType.BigInt},
            {DbType.AnsiStringFixedLength, SqlDbType.Char},
            {DbType.Single, SqlDbType.Real},
            {DbType.Double, SqlDbType.Float},
            {DbType.Decimal, SqlDbType.Decimal},
            {DbType.Boolean, SqlDbType.Bit},
            {DbType.Xml, SqlDbType.Xml},
            {DbType.String, SqlDbType.VarChar},
            {DbType.AnsiString, SqlDbType.VarChar},
            {DbType.StringFixedLength, SqlDbType.NChar},
            {DbType.DateTime, SqlDbType.DateTime},
            {DbType.Date, SqlDbType.Date},
            {DbType.DateTimeOffset, SqlDbType.DateTimeOffset},
            {DbType.Binary, SqlDbType.VarBinary}
        };

        #endregion

        #region  Métodos

        /// <summary>
        ///     Inclui separador numa lista de string
        ///     <para>ex: campo1,campo2, [ , ] virgula é um separador</para>
        /// </summary>
        /// <param name="separador">Tipo do separador</param>
        /// <param name="campo">Lista contendo uma strings</param>
        /// <returns></returns>
        private static string JoinString(string separador, IEnumerable<string> campo)
        {
            return string.Join<string>(separador, campo);
        }

        /// <summary>
        ///     Pesquisa se o objeto possui valor vazio ou 0 (zero)
        /// </summary>
        /// <param name="o">Um objeto</param>
        /// <returns></returns>
        private static bool IsEmptyOrZero(object o)
        {
            return o.Equals("") || o.Equals(0);
        }

        private void Clear()
        {
            _campoInsert.Clear(); //armazena um array de campos do insert
            _campoUpdate.Clear(); //armazena um array de campos do update
            _orderby.Clear(); //armazenas campos order by
            _parameters.Clear(); //armazena parametros
            _pksDelete.Clear(); //armazena um array de campos chaves do insert
            _pksInsert.Clear(); //armazena um array de campos chaves do insert
            _pksUpdate.Clear(); //armazena um array de campos chaves do update
            _valorInsert.Clear(); //armazena um array de campos do insert
        }

        #endregion
    }
}