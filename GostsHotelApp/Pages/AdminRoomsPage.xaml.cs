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
    /// Логика взаимодействия для AdminRoomsPage.xaml
    /// </summary>
    public partial class AdminRoomsPage : Page
    {
        public AdminRoomsPage()
        {
            InitializeComponent();
            LoadRooms();
        }

        private void LoadRooms()
        {
            var rooms = Dbb.hotelGostsDbEntities.Rooms.ToList();
            dgRooms.ItemsSource = rooms;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            RoomEditWindow win = new RoomEditWindow(0);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
            LoadRooms();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRooms();
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int roomId = (int)btn.Tag;
            RoomEditWindow win = new RoomEditWindow(roomId);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
            LoadRooms();
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int roomId = (int)btn.Tag;
            if (MessageBox.Show("Удалить номер? Все связанные бронирования будут удалены.", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var room = Dbb.hotelGostsDbEntities.Rooms.FirstOrDefault(r => r.RoomID == roomId);
                if (room != null)
                {
                    Dbb.hotelGostsDbEntities.Rooms.Remove(room);
                    Dbb.hotelGostsDbEntities.SaveChanges();
                    LoadRooms();
                }
            }
        }
    }
}
