using GostsHotelApp.Pages;
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

namespace GostsHotelApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AdjustMenuByRole();
            ContentFrame.Navigate(new CatalogPage());
        }

        private void AdjustMenuByRole()
        {
            adminMenu.Visibility = Visibility.Collapsed;
            exorcistMenu.Visibility = Visibility.Collapsed;
            myBookingsMenuItem.Visibility = (App.CurrentUserRole == "Клиент") ? Visibility.Visible : Visibility.Collapsed;

            if (App.CurrentUserRole == "Администратор")
            {
                adminMenu.Visibility = Visibility.Visible;
            }
            else if (App.CurrentUserRole == "Экзорцист-администратор")
            {
                exorcistMenu.Visibility = Visibility.Visible;
            }
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CatalogPage());
        }

        private void MyBookings_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new MyBookingsPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new ProfilePage());
        }

        private void AdminRooms_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new AdminRoomsPage());
        }

        private void AdminServices_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new AdminServicesPage());
        }

        private void AdminUsers_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new AdminUsersPage());
        }

        private void Evictions_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new EvictionsPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUserID = 0;
            App.CurrentUserRole = "";
            App.CurrentUserName = "";
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.CurrentUserID == 0)
                Application.Current.Shutdown();
        }
    }
}
