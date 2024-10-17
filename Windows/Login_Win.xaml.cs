using MySql.Data.MySqlClient;
using Proekt.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proekt.Windows
{
    public partial class Login_Win : Window
    {
        Database db = new Database();
        MySqlCommand cmd = new MySqlCommand();
        MainWindow mainWin = new MainWindow();
        public Login_Win()
        {
            InitializeComponent();
        }

        private void logButt_Click(object sender, RoutedEventArgs e)
        {
            db.openConn();

            cmd = new MySqlCommand("SELECT * FROM USERS WHERE Login = @login;", db.statusConn());
            cmd.Parameters.AddWithValue("@login", logBox.Text);

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                if (reader["Password"].ToString().Equals(passBox.Text.ToString()))
                {
                    MessageBox.Show("Успешная авторизация");
                    Hide();
                    mainWin.Show();
                }
            }
            db.closeConn();
            reader.Close();
        }
    }
}
