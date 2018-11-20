using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Ado
{
    /// <summary>
    /// Obtem configurações correntes
    /// </summary>
    public static class CurrentConfig
    {
        /// <summary>
        /// String de conexao com o banco de dados
        /// </summary>
        public static string ConexaoString => ConfigurationManager.ConnectionStrings["Credenciamento"].ConnectionString;
    }
}
