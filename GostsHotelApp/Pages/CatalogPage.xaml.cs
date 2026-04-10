using GostsHotelApp.Connections;
using GostsHotelApp.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GostsHotelApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
            LoadRooms();
        }

        private void LoadRooms()
        {
            var rooms = Dbb.hotelGostsDbEntities.Rooms
                .Where(r => r.Status == "Available" || r.Status == "Occupied")
                .OrderBy(r => r.RoomNumber)
                .ToList();
            roomsList.ItemsSource = rooms;
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int roomId = (int)btn.Tag;
            RoomDetailsWindow details = new RoomDetailsWindow(roomId);
            details.ShowDialog();
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUserID == 0 || App.CurrentUserRole != "Клиент")
            {
                MessageBox.Show("Только авторизованные клиенты могут бронировать.", "Доступ запрещён", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Button btn = sender as Button;
            int roomId = (int)btn.Tag;
            BookingWindow booking = new BookingWindow(roomId);
            booking.ShowDialog();
            LoadRooms();
        }
    }

    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status = value as string;
            if (status == "Available") return "#4CAF50";
            if (status == "Occupied") return "#FF9800";
            if (status == "Repair") return "#F44336";
            return "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CanBookConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status = value as string;
            return status == "Available";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
