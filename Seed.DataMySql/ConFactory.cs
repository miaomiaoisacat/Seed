using MySql.Data.MySqlClient;
using Seed.Core.Interfaces;
using System.Data;

namespace Seed.DataMySql
{
    public class ConFactory : IConFactory
    {
        public string ConStr { get; set; }

        public IDbConnection CreateCon()
        {
            return new MySqlConnection(this.ConStr);
        }
    }
}
