using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Proekt.Class
{
    internal class Zapros_Parts
    {
        Database db = new Database();
        MySqlCommand cmd = new MySqlCommand();

        public List<Parts> GetParts()
        {
            List<Parts> returnTheese = new List<Parts>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM parts", db.statusConn());

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Parts parts = new Parts
                    {
                        article = reader.GetString(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetInt32(3),
                    };
                    returnTheese.Add(parts);
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
