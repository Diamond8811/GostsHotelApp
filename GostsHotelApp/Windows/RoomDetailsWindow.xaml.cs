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
    /// Логика взаимодействия для RoomDetailsWindow.xaml
    /// </summary>
    public partial class RoomDetailsWindow : Window
    {
        public RoomDetailsWindow(int roomId)
        {
            InitializeComponent();
            LoadRoomDetails(roomId);
        }

        private void LoadRoomDetails(int roomId)
        {
            var room = Dbb.hotelGostsDbEntities.Rooms.FirstOrDefault(r => r.RoomID == roomId);
            if (room == null) return;

            tbRoomNumber.Text = room.RoomNumber;
            if (room.RoomTypes != null)
            {
                tbType.Text = room.RoomTypes.TypeName;
                tbDescription.Text = room.RoomTypes.Description;
            }

            var serviceIds = Dbb.hotelGostsDbEntities.Bookings
                .Where(b => b.RoomID == roomId)
                .SelectMany(b => b.BookingServices.Select(bs => bs.ServiceID))
                .Distinct()
                .ToList();

            var services = Dbb.hotelGostsDbEntities.Services
                .Where(s => serviceIds.Contains(s.ServiceID))
                .ToList();

            dgServices.ItemsSource = services;
        }
    }
}
