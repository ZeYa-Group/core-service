using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess
{
    public interface IDatabaseSecret
    {
        string GetConnectionString();
    }

    public class DatabaseSecret : IDatabaseSecret
    {
        private string host = "localhost";
        private string username = "postgres";
        private string password = "652431";
        private string database = "DEV-WORK";
        private string minPool = "1";

        public string GetConnectionString()
        {
            return $"Host={host};Username={username};Password={password};Database={database};MinPoolSize={minPool}";
        }
    }
}
