using System.Data;

namespace IMOD.Infra.Ado
{
    public static class CommandExtensions
    {
        /// <summary>
        ///     Executa um comando SQL, retirando parametros que não possuem clausula [where]
        ///     <para>
        ///         Para situações onde há parâmetros criados com o comando cmd.Parameters.Add (...), mas que na montagem da
        ///         clausula Where, não não parametros informados
        ///     </para>
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReaderSelect(this IDbCommand cmd)
        {
            //Verificar se clausula Where SQL após montada possui algum parametro,
            //caso contrário descartar todos os command parâmetros
            if (!cmd.CommandText.Contains("where")) cmd.Parameters.Clear();

            return cmd.ExecuteReader();
        }

        public static void CreateParameterSelect(this IDbCommand cmd, IDbDataParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter?.Value?.ToString())) return;

            cmd.Parameters.Add(parameter);
        }
    }
}