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
    public partial class DiagnosticWin : Window
    {
        public string logWin_logTxt = "";
        public string logWin_passTxt = "";

        Database db = new Database();

        private void updateTooltip(TextBlock textBlock)
        {
            var formattedText = new FormattedText(
            textBlock.Text,
            System.Globalization.CultureInfo.CurrentCulture,
            textBlock.FlowDirection,
            new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
            textBlock.FontSize,
            Brushes.Black,
            VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);

            if (textBlock.ActualWidth < formattedText.Width)
            {
                textBlock.ToolTip = textBlock.Text;
            }
            else
            {
                textBlock.ToolTip = null;
            }
        }

        public DiagnosticWin()
        {
            InitializeComponent();
        }

        private void diagn_Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void diagn_Grid_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void diagn_Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void reloadButt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (searchBox.Text != "Поиск")
            //{
            //    zapis_Grid.ItemsSource = zapros_Veh.Search(searchBox.Text);
            //}
        }

        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "Поиск")
            {
                searchBox.Text = "";
                searchBox.Style = (Style)Resources["Txt_Normal"];
            }
        }

        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchBox.Text = "Поиск";
                searchBox.Style = (Style)Resources["Txt_GrayItalic"];
            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                updateTooltip(textBlock);
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                textBlock.ToolTip = null;
            }
        }

        private void backButt_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWin = new MainWindow();
            Close();
            mainWin.Show(); // Кнопка возврата на главный экран
        }

        private void userButt_Click(object sender, RoutedEventArgs e)
        {
            Rectangle.Visibility = Visibility.Visible;
            UserPanel.Visibility = Visibility.Visible;

            try
            {
                db.openConn();

                MySqlCommand cmd = new MySqlCommand("SELECT lastName, firstName, patronomic FROM users WHERE (login = @login AND password = @password)", db.statusConn());
                cmd.Parameters.AddWithValue("@login", logWin_logTxt);
                cmd.Parameters.AddWithValue("@password", logWin_passTxt); // Получаем данные ФИО о пользователе

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lName_BoxUser.Text = reader.GetString(0);
                    fName_BoxUser.Text = reader.GetString(1);
                    pName_BoxUser.Text = reader.GetString(2);
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
        private void exitButt_Click(object sender, RoutedEventArgs e)
        {
            LoginWin loginWin = new LoginWin();
            Close();
            loginWin.Show(); // Кнопка выхода из приложения (возврата на окно авторизации)
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle.Visibility = Visibility.Hidden;
            UserPanel.Visibility = Visibility.Hidden;
        }

        private void calendarButt_Click(object sender, RoutedEventArgs e)
        {
            DateWin dateWin = new DateWin();
            string[] dates = dateBlock.Text.Split(new[] { "по" }, StringSplitOptions.RemoveEmptyEntries);

            if (dates.Length == 2)
            {
                if (DateTime.TryParse(dates[0].Replace("С", "").Trim(), out DateTime startDate) && DateTime.TryParse(dates[1].Trim(), out DateTime endDate))
                {
                    dateWin.StartDatePicker.SelectedDate = startDate;
                    dateWin.EndDatePicker.SelectedDate = endDate;
                }
            }

            if (dateWin.ShowDialog() == true)
            {
                string durationText = $"С {dateWin.StartDatePicker.SelectedDate.Value.ToShortDateString()} " +
                                      $"по {dateWin.EndDatePicker.SelectedDate.Value.ToShortDateString()}";
                dateBlock.Text = durationText;
                dateBlock.Style = (Style)FindResource("Txt_NormalBlock");
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void addButt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void doneButt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
