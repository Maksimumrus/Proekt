using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Proekt.Class
{
    internal class Zapros_Delaers
    {
        Database db = new Database();
        MySqlCommand cmd = new MySqlCommand();

        public void GetDealers(ComboBox comboBox)
        {
            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM dealers", db.statusConn());
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string dealerInfo = $"{reader.GetString(0)} - {reader.GetString(1)} - {reader.GetString(2)}";
                    comboBox.Items.Add(dealerInfo);
                }
                db.closeConn();
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
        }

        public List<DealerParts> GetParts(string name)
        {
            List<DealerParts> returnTheese = new List<DealerParts>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM dealer_parts WHERE dealers_name = @name", db.statusConn());
                cmd.Parameters.AddWithValue("@name", name);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnTheese.Add(new DealerParts
                    {
                        article = reader.GetString(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetInt32(3)
                    });
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

        public List<DealerParts> GetParts(string name, string content)
        {
            List<DealerParts> returnTheese = new List<DealerParts>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM dealer_parts WHERE dealers_name = @name AND article = @content;", db.statusConn());
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@content", content);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnTheese.Add(new DealerParts
                    {
                        article = reader.GetString(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetInt32(3)
                    });
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

        public List<DealerParts> Search(string name, string search)
        {
            List<DealerParts> returnTheese = new List<DealerParts>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM dealer_parts WHERE dealers_name = @Dname AND (article LIKE @article OR name LIKE @name)", db.statusConn());
                cmd.Parameters.AddWithValue("@Dname", name);
                cmd.Parameters.AddWithValue("@article", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", search));

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnTheese.Add(new DealerParts
                    {
                        article = reader.GetString(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetInt32(3),
                    });
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
