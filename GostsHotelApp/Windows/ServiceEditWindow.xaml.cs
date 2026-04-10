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
    /// Логика взаимодействия для ServiceEditWindow.xaml
    /// </summary>
    public partial class ServiceEditWindow : Window
    {
        private int editingServiceId;

        public ServiceEditWindow(int serviceId)
        {
            InitializeComponent();
            editingServiceId = serviceId;
            if (serviceId != 0)
                LoadServiceData(serviceId);
        }

        private void LoadServiceData(int serviceId)
        {
            var service = Dbb.hotelGostsDbEntities.Services.FirstOrDefault(s => s.ServiceID == serviceId);
            if (service != null)
            {
                txtServiceName.Text = service.ServiceName;
                txtPrice.Text = service.Price.ToString();
                txtUnit.Text = service.Unit;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServiceName.Text))
            {
                MessageBox.Show("Введите название услуги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtPrice.Text, out int price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                MessageBox.Show("Введите единицу измерения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Services service;
            if (editingServiceId == 0)
            {
                service = new Services();
                Dbb.hotelGostsDbEntities.Services.Add(service);
            }
            else
            {
                service = Dbb.hotelGostsDbEntities.Services.FirstOrDefault(s => s.ServiceID == editingServiceId);
                if (service == null) return;
            }

            service.ServiceName = txtServiceName.Text;
            service.Price = price;
            service.Unit = txtUnit.Text;

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
