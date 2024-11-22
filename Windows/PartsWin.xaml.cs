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
    public partial class PartsWin : Window
    {
        public string logWin_logTxt = "";
        public string logWin_passTxt = "";

        List<String> originalTexts = new List<String>();
        TextBox[] textBoxes;

        Database db = new Database();
        Zapros_Parts zapros_Parts = new Zapros_Parts();

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

        void Texts()
        {
            originalTexts.Add(name_Box.Text);
            originalTexts.Add(amount_Box.Text);
            originalTexts.Add(price_Box.Text);
        }

        public PartsWin()
        {
            InitializeComponent();
            Texts();

            parts_Grid.ItemsSource = zapros_Parts.GetParts();

            textBoxes = new TextBox[] {name_Box, amount_Box, price_Box};
            for (int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i].Text = originalTexts[i];
            } // Заполняем массив текстами, которые заданы при инициализации окна
        }

        private void reloadButt_Click(object sender, RoutedEventArgs e)
        {
            parts_Grid.ItemsSource = zapros_Parts.GetParts();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchBox.Text != "Поиск")
            {
                parts_Grid.ItemsSource = zapros_Parts.Search(searchBox.Text);
            }
        }

        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "Поиск")
            {
                searchBox.Text = "";
                searchBox.Style = (Style)FindResource("Txt_Normal");
            }
        }

        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchBox.Text = "Поиск";
                searchBox.Style = (Style)FindResource("Txt_GrayItalic");
            }
        }

        private void parts_Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName) // Изменяем названия колонок
            {
                case "article":
                    e.Column.Header = "Артикул";
                    break;
                case "name":
                    e.Column.Header = "Наименование";
                    break;
                case "amount":
                    e.Column.Header = "Количество";
                    break;
                case "price":
                    e.Column.Header = "Цена за шт.";
                    break;
                case "total":
                    e.Column.Header = "Итог";
                    break;
            }
        }

        private void parts_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (parts_Grid.SelectedCells.Count > 0)
            {
                foreach (var child in Panel_Left.Children)
                {
                    if (child is TextBox textBox)
                    {
                        textBox.Style = (Style)FindResource("Txt_Normal");
                    }
                } // Изменяем стиль TextBox


                var selectedCell = parts_Grid.SelectedCells[0];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                var cellValue = (cellContent as TextBlock)?.Text; // Получаем номер строки, по которой нажал пользователь, а также содержимое 0 ячейки

                var parts = zapros_Parts.GetParts(cellValue);

                if (parts.Count > 0)
                {
                    var part = parts[0];

                    name_Box.Text = part.name;
                    amount_Box.Text = part.amount.ToString();
                    price_Box.Text = part.price.ToString();
                } // Заполняем поля изменения информации

                var vehicles = zapros_Parts.GetVehicles(cellValue);

                ComboBox.Items.Clear();

                foreach (var vehicle in vehicles)
                {
                    ComboBox.Items.Add(vehicle.model);
                }

                if (vehicles.Count > 0)
                {
                    ComboBox.SelectedIndex = 0; 
                } // Добавляем подходящие к запчастям автомобили в выпадающий список
            }
        }

        private void parts_Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (i >= 0 && i < originalTexts.Count)
                {
                    textBoxes[i].Text = originalTexts[i];
                    textBoxes[i].Style = (Style)FindResource("Txt_GrayItalic");
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                int id = Array.IndexOf(textBoxes, textBox);

                if (id >= 0 && id < originalTexts.Count && textBox.Text == originalTexts[id])
                {
                    textBox.Text = "";
                    textBox.Style = (Style)FindResource("Txt_Normal");               
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                int id = Array.IndexOf(textBoxes, textBox);

                if (id >= 0 && id < originalTexts.Count && textBox.Text == "")
                {
                    textBox.Text = originalTexts[id];
                    textBox.Style = (Style)FindResource("Txt_GrayItalic");
                }                
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

        private void addButt_Click(object sender, RoutedEventArgs e)
        {
            DealersWin dealersWin = new DealersWin();
            dealersWin.ShowDialog();
        }

        private void editButt_Click(object sender, RoutedEventArgs e)
        {
            if (parts_Grid.SelectedCells.Count > 0) // Если пользователь нажал на строку
            {
                Parts part = new Parts // Создаем новый экземпляр класса Parts и заполняем его данными с полей
                {
                    name = name_Box.Text,
                    amount = Convert.ToInt32(amount_Box.Text),
                    price = Convert.ToInt32(price_Box.Text),
                };

                var selectedCell = parts_Grid.SelectedCells[0];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                var cellValue = (cellContent as TextBlock)?.Text; // Получеам номер строки, по которой нажал пользователь, а также содержимое 0 ячейки

                zapros_Parts.UpdateParts(part, cellValue);
                parts_Grid.ItemsSource = zapros_Parts.GetParts(); // Обновляем таблицу с данными
            }           
        }

        private void deleteButt_Click(object sender, RoutedEventArgs e)
        {
            if (parts_Grid.SelectedCells.Count > 0)
            {
                var selectedCell = parts_Grid.SelectedCells[0];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                var cellValue = (cellContent as TextBlock)?.Text;

                zapros_Parts.DeleteParts(cellValue);
                parts_Grid.ItemsSource = zapros_Parts.GetParts();
            }
        }
    }
}
