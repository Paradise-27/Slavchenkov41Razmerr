using Slavchenkov41Razmerr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Slavchenkov41Razmerr
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {

    private void UpdateServices()
        {
            var currentServices = Slavchenkov41Entities.GetContext().Product.ToList();
            var allProducts = currentServices;
            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100)).ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100)).ToList();
            }
            currentServices = currentServices.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            if (RButtonDown.IsChecked.Value)
            {
                currentServices = currentServices.OrderByDescending(p => p.ProductCost).ToList();
            }
            if (RButtonUp.IsChecked.Value)
            {
                currentServices = currentServices.OrderBy(p => p.ProductCost).ToList();
            }
            ServiceListView.ItemsSource = currentServices;
            TBCount.Text = "Кол-во " + currentServices.Count().ToString() + " из" + allProducts.Count.ToString();

        }
        public ProductPage(User user)
        {
            InitializeComponent();
            if (user != null)
            {
                FIOTB.Text = "Вы авторизованы как " + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Роль: " + "Администратор"; break;
                    case 2:
                        RoleTB.Text = "Роль: " + "Клиент"; break;
                    case 3:
                        RoleTB.Text = "Роль: " + "Менеджер"; break;

                }
            }
            else
            {
                FIOTB.Text = "Вы авторизованы как " +  "Гость";
                RoleTB.Text = "Роль: "+ "Гость";
            }
            
            var currentServices = Slavchenkov41Entities.GetContext().Product.ToList();
            ServiceListView.ItemsSource = currentServices;
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
    }
}
