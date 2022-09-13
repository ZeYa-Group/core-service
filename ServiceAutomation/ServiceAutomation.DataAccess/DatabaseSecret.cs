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
        private string port = "5432";
        private string username = "postgres";
        private string password = "21122012";
        private string database = "Trifecta"; //TrifectaProd - prod DB Name
        private string minPool = "1";

        public string GetConnectionString()
        {
            return $"Host={host};Port={port};Username={username};Password={password};Database={database};MinPoolSize={minPool}";
        }
    }
}
