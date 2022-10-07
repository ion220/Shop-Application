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
using Shop.Windows;

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void CreateOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var orderListWindow = new OrderListWindow();
            orderListWindow.ShowDialog();
        }

        private void DirectoriesBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.Frame.Navigate(new DirectoriesPage());
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            var assemblingOrderListWindow = new AssemblingOrderListWindow();
            assemblingOrderListWindow.ShowDialog();
        }
    }
}
