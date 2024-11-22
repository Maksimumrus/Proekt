using Proekt.Class;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace Proekt.Windows
{
    public partial class MainWindow : Window
    {
        List<String> originalTexts = new List<String>();

        public string logWin_logTxt = "";
        public string logWin_passTxt = "";

        private void Texts()
        {
            originalTexts.Add(lName_Box.Text);
            originalTexts.Add(fName_Box.Text);
            originalTexts.Add(pName_Box.Text);
            originalTexts.Add(pNum_Box.Text);
            originalTexts.Add(mailAddr_Box.Text);
            originalTexts.Add(addr_Box.Text);
        }

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

        Database db = new Database();
        Zapros_Veh zapros_Veh = new Zapros_Veh();

        public MainWindow()
        {
            InitializeComponent();
            zapis_Grid.ItemsSource = zapros_Veh.GetVeh(); // При загрузке окна таблица получает данные с метода GetVeh

            Texts();
        }

        private void reloadButt_Click(object sender, RoutedEventArgs e)
        {
            zapis_Grid.ItemsSource= zapros_Veh.GetVeh();
        }

        private void zapis_Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "licencePlate":
                    e.Column.Header = "Номерной знак";
                    break;
                case "marka":
                    e.Column.Header = "Марка";
                    break;
                case "model":
                    e.Column.Header = "Модель";
                    break;
                case "type":
                    e.Column.Header = "Тип";
                    break;
                case "category":
                    e.Column.Header = "Категория";
                    break;
                case "releaseYear":
                    e.Column.Header = "Год выпуска";
                    break;
                case "VIN_num":
                    e.Column.Header = "VIN номер";
                    break;
                case "engineModel":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "engineNum":
                    e.Column.Visibility= Visibility.Collapsed;
                    break;
                case "description":
                    e.Column.Header = "Описание проблемы";
                    break;
            }
        }

        private void zapis_Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Zapros_Clients zapros_Clients = new Zapros_Clients();

            if (zapis_Grid.SelectedCells.Count > 0)
            {
                foreach (var child in CPanel_Left.Children)
                {
                    if (child is TextBlock textBlock)
                    {
                        textBlock.Style = (Style)FindResource("Txt_Normal");
                    }
                }

                foreach (var child in CPanel_Right.Children)
                {
                    if (child is TextBlock textBlock)
                    {
                        textBlock.Style = (Style)FindResource("Txt_Normal");
                    }
                }
                var selectedCell = zapis_Grid.SelectedCells[0];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                var cellValue = (cellContent as TextBlock)?.Text;

                var clients = zapros_Clients.GetClients(cellValue);

                if (clients.Count > 0)
                {
                    var client = clients[0];

                    lName_Box.Text = client.lastName;
                    fName_Box.Text = client.firstName;
                    pName_Box.Text = client.patronomic;
                    pNum_Box.Text = client.phoneNum;
                    mailAddr_Box.Text = client.email;
                    addr_Box.Text = client.address;
                }
            }          
        }

        private void zapis_Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (var child in CPanel_Left.Children)
            {
                if (child is TextBlock textBlock)
                {
                    textBlock.Style = (Style)FindResource("Txt_GrayItalic");
                }
            }

            foreach (var child in CPanel_Right.Children)
            {
                if (child is TextBlock textBlock)
                {
                    textBlock.Style = (Style)FindResource("Txt_GrayItalic");
                }
            }

            lName_Box.Text = originalTexts[0];
            fName_Box.Text = originalTexts[1];
            pName_Box.Text = originalTexts[2];
            pNum_Box.Text = originalTexts[3];
            mailAddr_Box.Text = originalTexts[4];
            addr_Box.Text = originalTexts[5];
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchBox.Text != "Поиск")
            {
                zapis_Grid.ItemsSource = zapros_Veh.Search(searchBox.Text);
            }            
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

        private void userButt_Click(object sender, RoutedEventArgs e)
        {
            Rectangle.Visibility = Visibility.Visible;
            UserPanel.Visibility = Visibility.Visible;

            try
            {
                db.openConn();

                MySqlCommand cmd = new MySqlCommand("SELECT lastName, firstName, patronomic FROM users WHERE (login = @login AND password = @password)", db.statusConn());
                cmd.Parameters.AddWithValue("@login", logWin_logTxt);
                cmd.Parameters.AddWithValue("@password", logWin_passTxt);

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
            loginWin.Show();
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle.Visibility = Visibility.Hidden;
            UserPanel.Visibility = Visibility.Hidden;
        }

        private void partsButt_Click(object sender, RoutedEventArgs e)
        {
            PartsWin partsWin = new PartsWin();
            partsWin.logWin_logTxt = logWin_logTxt;
            partsWin.logWin_passTxt = logWin_passTxt;
            Close();
            partsWin.Show();
        }

        private void diagnButt_Click(object sender, RoutedEventArgs e)
        {
            DiagnosticWin diagnosticWin = new DiagnosticWin();
            diagnosticWin.logWin_logTxt = logWin_logTxt;
            diagnosticWin.logWin_passTxt = logWin_passTxt;
            Close();
            diagnosticWin.Show();
        }
    }
}