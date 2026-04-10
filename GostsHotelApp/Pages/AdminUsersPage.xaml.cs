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
    /// Логика взаимодействия для AdminUsersPage.xaml
    /// </summary>
    public partial class AdminUsersPage : Page
    {
        public AdminUsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = Dbb.hotelGostsDbEntities.Users.ToList();
            dgUsers.ItemsSource = users;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int userId = (int)btn.Tag;
            UserEditWindow win = new UserEditWindow(userId);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
            LoadUsers();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int userId = (int)btn.Tag;
            if (userId == App.CurrentUserID)
            {
                MessageBox.Show("Нельзя удалить самого себя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Удалить пользователя? Все его данные (бронирования, логи) будут удалены.", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var user = Dbb.hotelGostsDbEntities.Users.FirstOrDefault(u => u.UserID == userId);
                if (user != null)
                {
                    Dbb.hotelGostsDbEntities.Users.Remove(user);
                    Dbb.hotelGostsDbEntities.SaveChanges();
                    LoadUsers();
                }
            }
        }
    }
}
