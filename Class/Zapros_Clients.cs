using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proekt.Class
{
    class Zapros_Clients
    {
        Database db = new Database();
        MySqlCommand cmd = new MySqlCommand();
        public List<Clients> GetClients(string content)
        {
            List<Clients> returnTheese = new List<Clients>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT c.* FROM clients c JOIN vehicles v ON c.id = v.clients_id WHERE v.licencePlate = @content;", db.statusConn());
                cmd.Parameters.AddWithValue("content", content);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Clients clients = new Clients
                    {
                        ID = reader.GetInt32(0),
                        lastName = reader.GetString(1),
                        firsName = reader.GetString(2),
                        patronomic = reader.GetString(3),
                        phoneNum = reader.GetString(4),
                        email = reader.GetString(5),
                        address = reader.GetString(6)
                    };
                    returnTheese.Add(clients);
                }
                db.closeConn();
                return returnTheese;
            }
            catch
            {
                MessageBox.Show(
                    "Ошибка запроса",
                    "Невозможно выполнить запрос",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
            }
            return returnTheese;
        }
    }
}