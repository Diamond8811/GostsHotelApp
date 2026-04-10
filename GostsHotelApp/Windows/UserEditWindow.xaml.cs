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
    /// Логика взаимодействия для UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        private int editingUserId;

        public UserEditWindow(int userId)
        {
            InitializeComponent();
            editingUserId = userId;
            LoadRoles();
            LoadGhosts();
            if (userId != 0)
                LoadUserData(userId);
        }

        private void LoadRoles()
        {
            var roles = Dbb.hotelGostsDbEntities.Roles.ToList();
            cbRole.ItemsSource = roles;
        }

        private void LoadGhosts()
        {
            var ghosts = Dbb.hotelGostsDbEntities.Ghosts.ToList();
            var emptyGhost = new Ghosts() { GhostID = 0, Name = "(Без призрака)" };
            ghosts.Insert(0, emptyGhost);
            cbGhost.ItemsSource = ghosts;
            cbGhost.SelectedValuePath = "GhostID";
        }

        private void LoadUserData(int userId)
        {
            var user = Dbb.hotelGostsDbEntities.Users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                txtLogin.Text = user.Login;
                cbRole.SelectedValue = user.RoleID;
                cbGhost.SelectedValue = user.GhostID ?? 0;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbRole.SelectedValue == null)
            {
                MessageBox.Show("Выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = Dbb.hotelGostsDbEntities.Users.FirstOrDefault(u => u.UserID == editingUserId);
            if (user == null) return;

            user.Login = txtLogin.Text;
            if (!string.IsNullOrEmpty(txtPassword.Password))
                user.PasswordHash = txtPassword.Password;
            user.RoleID = (int)cbRole.SelectedValue;
            int ghostId = (int)cbGhost.SelectedValue;
            user.GhostID = ghostId == 0 ? (int?)null : ghostId;

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
