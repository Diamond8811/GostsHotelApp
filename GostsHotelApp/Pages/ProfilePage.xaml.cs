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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private Users currentUser;
        private Ghosts currentGhost;

        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            currentUser = Dbb.hotelGostsDbEntities.Users.FirstOrDefault(u => u.UserID == App.CurrentUserID);
            if (currentUser == null) return;

            tbLogin.Text = currentUser.Login;

            if (currentUser.GhostID != null)
            {
                currentGhost = Dbb.hotelGostsDbEntities.Ghosts.FirstOrDefault(g => g.GhostID == currentUser.GhostID);
                if (currentGhost != null)
                {
                    txtGhostName.Text = currentGhost.Name;
                    txtYear.Text = currentGhost.YearOfPassing.ToString();
                    txtTransparency.Text = currentGhost.TransparencyLevel.ToString();
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (currentGhost != null)
            {
                if (string.IsNullOrWhiteSpace(txtGhostName.Text))
                {
                    MessageBox.Show("Имя призрака не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtYear.Text, out int year) || year < 500 || year > System.DateTime.Now.Year)
                {
                    MessageBox.Show("Некорректный год упокоения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtTransparency.Text, out int transparency) || transparency < 0 || transparency > 100)
                {
                    MessageBox.Show("Прозрачность должна быть от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                currentGhost.Name = txtGhostName.Text;
                currentGhost.YearOfPassing = year;
                currentGhost.TransparencyLevel = transparency;
            }

            string newPass = txtNewPassword.Password;
            if (!string.IsNullOrEmpty(newPass))
            {
                currentUser.PasswordHash = newPass;
            }

            Dbb.hotelGostsDbEntities.SaveChanges();
            MessageBox.Show("Данные обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
