﻿using MySql.Data.MySqlClient;
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
    public partial class LoginWin : Window
    {
        Database db = new Database();
        MySqlCommand cmd = new MySqlCommand();

        private async void incorrTxt_Show() // Функция показа текста при неккоректном вводе для полей
        {
            incorrTxt.Visibility = Visibility.Visible;
            await Task.Delay(5000);
            incorrTxt.Visibility = Visibility.Hidden;
        }

        private void logBox_TXT()
        {
            if (logBox.Text == "")
            {
                logBox.Text = "Логин";
                logBox.FontStyle = FontStyles.Italic;
                logBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        public LoginWin()
        {
            InitializeComponent();
            incorrTxt.Visibility = Visibility.Hidden;
            logBox_TXT();
        }

        private void logButt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.openConn();

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataTable table = new DataTable();
                cmd = new MySqlCommand("SELECT login, password FROM USERS WHERE (login = @login AND password = @password);", db.statusConn());
                cmd.Parameters.AddWithValue("@login", logBox.Text);
                cmd.Parameters.AddWithValue("@password", passBox.Password);

                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    MainWindow mainWin = new MainWindow();
                    MessageBox.Show(
                    "Авторизация прошла успешно",
                    "Успешная авторизация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                    Close();
                    mainWin.logWin_logTxt = logBox.Text;
                    mainWin.logWin_passTxt = passBox.Password;
                    mainWin.Show();
                }
                else { incorrTxt_Show(); }

                db.closeConn();
            }
            catch
            {
                MessageBox.Show(
                    "Невозможно подключиться к серверу",
                    "Сервер недоступен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
            }
        }

        private void logBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (logBox.Text == "Логин")
            {
                logBox.Text = "";
                logBox.FontStyle = FontStyles.Normal;
                logBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void logBox_LostFocus(object sender, RoutedEventArgs e)
        {
            logBox_TXT();
        }

        private void logButt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                logButt_Click(sender, e);
            }
        }
    }
}
