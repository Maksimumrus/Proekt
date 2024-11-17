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

        public List<Parts> GetParts() // Получаем данные о запчастях
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
                        price = reader.GetDouble(3),
                        total = reader.GetDouble(4),
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

        public List<Parts> GetParts(string content) // Получаем данные о запчастях для заполнонения полей изменения информации
        {
            List<Parts> returnTheese = new List<Parts>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM parts WHERE article = @content", db.statusConn());
                cmd.Parameters.AddWithValue("@content", content);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Parts parts = new Parts
                    {
                        article = reader.GetString(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetDouble(3),
                        total = reader.GetDouble(4),
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

        public void UpdateParts(Parts editParts, string content) // Метод обновления данных о запчастях
        {
            List<Parts> editPart = new List<Parts>();
            try
            {
                db.openConn();

                cmd = new MySqlCommand("UPDATE parts SET name = @name, amount = @amount, price = @price WHERE article = @content", db.statusConn());
                cmd.Parameters.AddWithValue("@name", editParts.name);
                cmd.Parameters.AddWithValue("@amount", editParts.amount);
                cmd.Parameters.AddWithValue("@price", editParts.price);
                cmd.Parameters.AddWithValue("@content", content);

                cmd.ExecuteNonQuery();

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

        public void DeleteParts(string content) // Метод удаления данных о запчастях
        {
            try
            {
                db.openConn();

                cmd = new MySqlCommand("DELETE FROM parts WHERE article = @content", db.statusConn());
                cmd.Parameters.AddWithValue("@content", content);

                cmd.ExecuteNonQuery();

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

        public List<Vehicles> GetVehicles(string content) // Метод получения данных об автомобилях, к которым подходит данная запчасть
        {
            List<Vehicles> returnTheese = new List<Vehicles>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT v.* FROM vehicles v JOIN parts_vehicles pv ON v.model = pv.vehicles_model " +
                    "JOIN parts p ON pv.parts_article = p.article WHERE p.article = @content;", db.statusConn());
                cmd.Parameters.AddWithValue("@content", content);

                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    Vehicles vehicle = new Vehicles
                    {
                        marka = reader.GetString(0),
                        model = reader.GetString(1),
                        type = reader.GetString(2),
                        releaseYear = reader.GetInt32(3),
                        engineModel = reader.GetString(4),
                    };
                    returnTheese.Add(vehicle);
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

        public List<Parts> Search(string search) // Метод поиска
        {
            List<Parts> returnTheese = new List<Parts>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM parts WHERE article LIKE @article OR name LIKE @name", db.statusConn());
                cmd.Parameters.AddWithValue("@article", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", search));

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Parts parts = new Parts
                    {
                        article = reader.GetString(0),
                        name = reader.GetString(1),
                        amount = reader.GetInt32(2),
                        price = reader.GetDouble(3),
                        total = reader.GetDouble(4),
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
