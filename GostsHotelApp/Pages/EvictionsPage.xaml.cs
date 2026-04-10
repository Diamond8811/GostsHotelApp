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
using Microsoft.VisualBasic;

namespace GostsHotelApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EvictionsPage.xaml
    /// </summary>
    public partial class EvictionsPage : Page
    {
        public EvictionsPage()
        {
            InitializeComponent();
            LoadActiveBookings();
        }

        private void LoadActiveBookings()
        {
            var active = Dbb.hotelGostsDbEntities.Bookings
                .Where(b => b.Status == "Active")
                .ToList();
            dgActiveBookings.ItemsSource = active;
        }

        private void Evict_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int bookingId = (int)btn.Tag;
            var booking = Dbb.hotelGostsDbEntities.Bookings.FirstOrDefault(b => b.BookingID == bookingId);
            if (booking == null) return;

            string reason = Microsoft.VisualBasic.Interaction.InputBox("Причина выселения:", "Выселение", "Нарушение правил");
            if (string.IsNullOrWhiteSpace(reason)) return;

            booking.Status = "Evicted";
            Evictions ev = new Evictions()
            {
                BookingID = bookingId,
                EvictionDate = DateTime.Now.Date,
                Reason = reason,
                ExecutorUserID = App.CurrentUserID
            };
            Dbb.hotelGostsDbEntities.Evictions.Add(ev);
            Dbb.hotelGostsDbEntities.SaveChanges();

            MessageBox.Show("Призрак выселен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadActiveBookings();
        }
    }
}
