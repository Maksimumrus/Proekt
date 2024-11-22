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
    public partial class AddPartsWin : Window
    {
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

        public AddPartsWin()
        {
            parts_Grid.ItemsSource = zapros_Parts.GetParts();
            InitializeComponent();
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

        private void parts_Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void parts_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void deleteButt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backButt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
