using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proekt.Class
{
    class Zapros_Veh
    {
        Database db = new Database();
        MySqlCommand cmd = new MySqlCommand();

        public List<Clients_Vehicles> GetVeh() // Получение записей об автомобилях
        {
            List<Clients_Vehicles> returnTheese = new List<Clients_Vehicles>();
            try
            {
                db.openConn(); // Открываем подключение к базе данных

                cmd = new MySqlCommand("SELECT * FROM clients_vehicles", db.statusConn());
                MySqlDataReader reader = cmd.ExecuteReader(); // Служит для преобразования данных с сервера в данные языка программирования

                while (reader.Read())
                {
                    Clients_Vehicles c_vehicles = new Clients_Vehicles // Добавляем полученные с сервера данные в экземпляр класса
                    {
                        licencePlate = reader.GetString(0),
                        marka = reader.GetString(1),
                        model = reader.GetString(2),
                        type = reader.GetString(3),
                        category = reader.GetString(4),
                        releaseYear = reader.GetInt32(5),
                        VIN_num = reader.GetString(6),
                        engineModel = reader.GetString(7),
                        engineNum = reader.GetString(8),
                        description = reader.GetString(9),
                    };
                    returnTheese.Add(c_vehicles);
                }

                db.closeConn(); // Закрываем подключение к базе данных
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

        public List<Clients_Vehicles> Search(string search)
        {
            List<Clients_Vehicles> returnTheese = new List<Clients_Vehicles>();

            try
            {
                db.openConn();

                cmd = new MySqlCommand("SELECT * FROM clients_vehicles WHERE licencePlate LIKE @lPLate_Search OR marka LIKE @marka_Search OR model LIKE @model_Search" +
                    " OR type LIKE @type_Search OR category LIKE @category_Search OR releaseYear LIKE @relYear_Search;", db.statusConn());

                cmd.Parameters.AddWithValue("@lPlate_Search", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@marka_Search", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@model_Search", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@type_Search", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@category_Search", string.Format("%{0}%", search));
                cmd.Parameters.AddWithValue("@relYear_Search", string.Format("%{0}%", search));

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Clients_Vehicles c_vehicles = new Clients_Vehicles
                    {
                        licencePlate = reader.GetString(0),
                        marka = reader.GetString(1),
                        model = reader.GetString(2),
                        type = reader.GetString(3),
                        category = reader.GetString(4),
                        releaseYear = reader.GetInt32(5),
                        VIN_num = reader.GetString(6),
                        engineModel = reader.GetString(7),
                        engineNum = reader.GetString(8),
                        description = reader.GetString(9)
                    };
                    returnTheese.Add(c_vehicles);
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
