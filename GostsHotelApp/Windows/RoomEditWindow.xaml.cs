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
    /// Логика взаимодействия для RoomEditWindow.xaml
    /// </summary>
    public partial class RoomEditWindow : Window
    {
        private int editingRoomId;

        public RoomEditWindow(int roomId)
        {
            InitializeComponent();
            editingRoomId = roomId;
            LoadTypes();
            if (roomId != 0)
                LoadRoomData(roomId);
        }

        private void LoadTypes()
        {
            var types = Dbb.hotelGostsDbEntities.RoomTypes.ToList();
            cbType.ItemsSource = types;
        }

        private void LoadRoomData(int roomId)
        {
            var room = Dbb.hotelGostsDbEntities.Rooms.FirstOrDefault(r => r.RoomID == roomId);
            if (room != null)
            {
                txtRoomNumber.Text = room.RoomNumber;
                cbType.SelectedValue = room.TypeID;
                txtPrice.Text = room.PricePerNight.ToString();
                cbStatus.Text = room.Status;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                MessageBox.Show("Введите номер комнаты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtPrice.Text, out int price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbType.SelectedValue == null)
            {
                MessageBox.Show("Выберите тип номера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int typeId = (int)cbType.SelectedValue;
            string status = (cbStatus.SelectedItem as ComboBoxItem).Content.ToString();

            Rooms room;
            if (editingRoomId == 0)
            {
                room = new Rooms();
                Dbb.hotelGostsDbEntities.Rooms.Add(room);
            }
            else
            {
                room = Dbb.hotelGostsDbEntities.Rooms.FirstOrDefault(r => r.RoomID == editingRoomId);
                if (room == null) return;
            }

            room.RoomNumber = txtRoomNumber.Text;
            room.TypeID = typeId;
            room.PricePerNight = price;
            room.Status = status;

            Dbb.hotelGostsDbEntities.SaveChanges();
            MessageBox.Show("Сохранено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
