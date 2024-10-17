using MySql.Data.MySqlClient;
using Proekt.Class;
using System;
using System.Collections.Generic;
using System.Data;
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

        private async void incorrTxt_Show() // Функция показа текста при неккоректном вводе для полей
        {
            incorrTxt.Visibility = Visibility.Visible;
            await Task.Delay(5000);
            incorrTxt.Visibility = Visibility.Hidden;
        }

        public Login_Win()
        {
            InitializeComponent();
            incorrTxt.Visibility = Visibility.Hidden;
        }

        private void logButt_Click(object sender, RoutedEventArgs e)
        {
            db.openConn();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            cmd = new MySqlCommand("SELECT * FROM USERS WHERE (Login = @login AND Password = @password);", db.statusConn());
            cmd.Parameters.AddWithValue("@login", logBox.Text);
            cmd.Parameters.AddWithValue("@password", passBox.Password);

            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Успешная авторизация");
                Hide();
                mainWin.Show();
            }
            else { incorrTxt_Show(); }

            db.closeConn();
        }

        private void logBox_GotFocus(object sender, RoutedEventArgs e)
        {
            logBox.Text = "";
        }

        private void logBox_LostFocus(object sender, RoutedEventArgs e)
        {
            logBox.Text = "Введите логин";
            logBox.Foreground = "Gray";
        }
    }
}
