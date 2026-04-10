using GostsHotelApp.Connections;
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
    /// Логика взаимодействия для MyBookingsPage.xaml
    /// </summary>
    public partial class MyBookingsPage : Page
    {
        public MyBookingsPage()
        {
            InitializeComponent();
            LoadMyBookings();
        }

        private void LoadMyBookings()
        {
            var user = Dbb.hotelGostsDbEntities.Users.FirstOrDefault(u => u.UserID == App.CurrentUserID);
            if (user == null || user.GhostID == null) return;

            var bookings = Dbb.hotelGostsDbEntities.Bookings
                .Where(b => b.GhostID == user.GhostID)
                .OrderByDescending(b => b.CheckInDate)
                .ToList();
            dgBookings.ItemsSource = bookings;
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int bookingId = (int)btn.Tag;
            var booking = Dbb.hotelGostsDbEntities.Bookings.FirstOrDefault(b => b.BookingID == bookingId);
            if (booking != null && booking.Status == "Active")
            {
                booking.Status = "Cancelled";
                Dbb.hotelGostsDbEntities.SaveChanges();
                LoadMyBookings();
                MessageBox.Show("Бронирование отменено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    public class CancelVisibleConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status = value as string;
            return status == "Active" ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
