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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string ghostName = txtGhostName.Text.Trim();
            string yearStr = txtYear.Text.Trim();
            string transStr = txtTransparency.Text.Trim();
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(ghostName) || string.IsNullOrEmpty(yearStr) || string.IsNullOrEmpty(transStr) ||
                string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(yearStr, out int year) || year < 500 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Год упокоения должен быть от 500 до текущего года.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(transStr, out int transparency) || transparency < 0 || transparency > 100)
            {
                MessageBox.Show("Прозрачность должна быть числом от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Dbb.hotelGostsDbEntities.Users.Any(u => u.Login == login))
            {
                MessageBox.Show("Логин уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Ghosts newGhost = new Ghosts()
            {
                Name = ghostName,
                YearOfPassing = year,
                TransparencyLevel = transparency,
                RegistrationDate = DateTime.Now.Date
            };
            Dbb.hotelGostsDbEntities.Ghosts.Add(newGhost);
            Dbb.hotelGostsDbEntities.SaveChanges();

            int roleClient = Dbb.hotelGostsDbEntities.Roles.First(r => r.RoleName == "Клиент").RoleID;

            Users newUser = new Users()
            {
                Login = login,
                PasswordHash = password,
                RoleID = roleClient,
                GhostID = newGhost.GhostID,
                CreatedAt = DateTime.Now.Date
            };
            Dbb.hotelGostsDbEntities.Users.Add(newUser);
            Dbb.hotelGostsDbEntities.SaveChanges();

            MessageBox.Show("Регистрация успешна! Теперь можете войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
