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
    /// Логика взаимодействия для AdminServicesPage.xaml
    /// </summary>
    public partial class AdminServicesPage : Page
    {
        public AdminServicesPage()
        {
            InitializeComponent();
            LoadServices();
        }

        private void LoadServices()
        {
            var services = Dbb.hotelGostsDbEntities.Services.ToList();
            dgServices.ItemsSource = services;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ServiceEditWindow win = new ServiceEditWindow(0);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
            LoadServices();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadServices();
        }

        private void EditService_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int serviceId = (int)btn.Tag;
            ServiceEditWindow win = new ServiceEditWindow(serviceId);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
            LoadServices();
        }

        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int serviceId = (int)btn.Tag;
            if (MessageBox.Show("Удалить услугу? Это может повлиять на существующие бронирования.", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var service = Dbb.hotelGostsDbEntities.Services.FirstOrDefault(s => s.ServiceID == serviceId);
                if (service != null)
                {
                    Dbb.hotelGostsDbEntities.Services.Remove(service);
                    Dbb.hotelGostsDbEntities.SaveChanges();
                    LoadServices();
                }
            }
        }
    }
}
