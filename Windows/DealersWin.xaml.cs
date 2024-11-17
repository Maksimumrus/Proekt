using Proekt.Class;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Proekt.Windows
{
    public partial class DealersWin : Window
    {
        Database db = new Database();
        Zapros_Delaers zapros_Dealers = new Zapros_Delaers();
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
        public DealersWin()
        {
            InitializeComponent();

            zapros_Dealers.GetDealers(dealerBox);
            dealerBox.SelectedIndex = 0;
        }

        private void reloadButt_Click(object sender, RoutedEventArgs e)
        {
            if (dealerParts_Grid.Items.Count > 0)
            {
                string selectedDealer = dealerBox.SelectedItem.ToString();
                string dealerName = selectedDealer.Split('-')[0].Trim();
                dealerParts_Grid.ItemsSource = zapros_Dealers.GetParts(dealerName);
            }
        }

        private void dealerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedDealer = dealerBox.SelectedItem.ToString();
            string dealerName = selectedDealer.Split('-')[0].Trim();
            dealerParts_Grid.ItemsSource = zapros_Dealers.GetParts(dealerName);
        }

        private void dealerParts_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            priceBox.Style = (Style)FindResource("Txt_NormalBlock");

            var selectedCell = dealerParts_Grid.SelectedCells[0];
            var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
            var cellValue = (cellContent as TextBlock)?.Text;

            string selectedDealer = dealerBox.SelectedItem.ToString();
            string dealerName = selectedDealer.Split('-')[0].Trim();

            var Dparts = zapros_Dealers.GetParts(dealerName, cellValue);

            if (Dparts.Count > 0)
            {
                var Dpart = Dparts[0];

                priceBox.Text = Dpart.price.ToString();
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchBox.Text != "Поиск")
            {
                string selectedDealer = dealerBox.SelectedItem.ToString();
                string dealerName = selectedDealer.Split('-')[0].Trim();
                dealerParts_Grid.ItemsSource = zapros_Dealers.Search(dealerName, searchBox.Text);
            }
            else { reloadButt_Click(sender, e); }           
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
            Close(); // Кнопка возврата на главный экран
        }

        private void amountBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (amountBox.Text == "Количество для заказа")
            {
                amountBox.Style = (Style)FindResource("Txt_Normal");
                amountBox.Text = "";
            }
        }

        double initialPrice;
        private void amountBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (priceBox == null)
            {
                return;
            }

            if (initialPrice == 0 && !string.IsNullOrEmpty(priceBox.Text) && priceBox.Text != "Итоговая стоимость")
            {
                initialPrice = Convert.ToDouble(priceBox.Text);
            }

            if (string.IsNullOrEmpty(amountBox.Text) || amountBox.Text == "Количество для заказа")
            {
                priceBox.Text = initialPrice.ToString();
                return;
            }

            int amount;
            if (int.TryParse(amountBox.Text, out amount) && amount > 0)
            {
                double price = initialPrice;
                if (amount != 1)
                {
                    price = amount * initialPrice * 1.05;
                }
                priceBox.Text = price.ToString();
            }
        }
    }
}