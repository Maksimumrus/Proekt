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
    public partial class DateWin : Window
    {
        public DateWin()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate.HasValue && endDate.HasValue)
            {
                // Проверка, что конечная дата не меньше текущей
                if (endDate < DateTime.Today)
                {
                    MessageBox.Show("Конечная дата не может быть меньше текущей даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка, что дата окончания не меньше даты начала
                if (endDate < startDate)
                {
                    MessageBox.Show("Дата окончания не может быть меньше даты начала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Если все проверки пройдены, закрываем окно
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите обе даты.");
            }
        }
    }
}
