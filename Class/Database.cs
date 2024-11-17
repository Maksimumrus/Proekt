using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Proekt.Class
{
    internal class Database
    {
        MySqlConnection connection = new MySqlConnection("server = localhost; port = 3308; username = root; password = root; database = proekt");
        public void openConn()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConn()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection statusConn()
        {
            return connection;
        }
    }
}
