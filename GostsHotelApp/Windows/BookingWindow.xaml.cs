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
using System.Windows.Shapes;

namespace GostsHotelApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        private int roomId;
        private int pricePerNight;

        public BookingWindow(int roomId)
        {
            InitializeComponent();
            this.roomId = roomId;
            LoadRoomInfo();
            LoadServices();
            dpCheckIn.SelectedDate = DateTime.Now.AddDays(1);
            dpCheckOut.SelectedDate = DateTime.Now.AddDays(2);
        }

        private void LoadRoomInfo()
        {
            var room = Dbb.hotelGostsDbEntities.Rooms.FirstOrDefault(r => r.RoomID == roomId);
            if (room != null)
            {
                tbRoomNumber.Text = room.RoomNumber;
                pricePerNight = room.PricePerNight;
            }
        }

        private void LoadServices()
        {
            var services = Dbb.hotelGostsDbEntities.Services.ToList();
            lbServices.ItemsSource = services;
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            if (dpCheckIn.SelectedDate == null || dpCheckOut.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты заезда и выезда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime checkIn = dpCheckIn.SelectedDate.Value;
            DateTime checkOut = dpCheckOut.SelectedDate.Value;

            if (checkIn >= checkOut)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isAvailable = !Dbb.hotelGostsDbEntities.Bookings.Any(b =>
                b.RoomID == roomId &&
                b.Status != "Cancelled" && b.Status != "Evicted" &&
                ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||
                 (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
                 (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate)));

            if (!isAvailable)
            {
                MessageBox.Show("Номер уже занят на выбранные даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int days = (checkOut - checkIn).Days;
            int totalPrice = days * pricePerNight;

            var currentUser = Dbb.hotelGostsDbEntities.Users.FirstOrDefault(u => u.UserID == App.CurrentUserID);
            if (currentUser == null || currentUser.GhostID == null)
            {
                MessageBox.Show("Ваш профиль не связан с призраком. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Bookings newBooking = new Bookings()
            {
                GhostID = currentUser.GhostID.Value,
                RoomID = roomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                TotalPrice = totalPrice,
                Status = "Active",
                CreatedAt = DateTime.Now.Date
            };
            Dbb.hotelGostsDbEntities.Bookings.Add(newBooking);
            Dbb.hotelGostsDbEntities.SaveChanges();

            foreach (Services service in lbServices.SelectedItems)
            {
                BookingServices bs = new BookingServices()
                {
                    BookingID = newBooking.BookingID,
                    ServiceID = service.ServiceID,
                    Quantity = 1,
                    PriceAtTime = service.Price
                };
                Dbb.hotelGostsDbEntities.BookingServices.Add(bs);
            }
            Dbb.hotelGostsDbEntities.SaveChanges();

            MessageBox.Show($"Бронирование оформлено! Общая стоимость: {totalPrice + lbServices.SelectedItems.Count * 50} эктоплазмы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
