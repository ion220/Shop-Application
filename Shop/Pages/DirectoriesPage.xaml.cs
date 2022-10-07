using Shop.Windows;
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

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DirectoriesPage.xaml
    /// </summary>
    public partial class DirectoriesPage : Page
    {
        public DirectoriesPage()
        {
            InitializeComponent();
        }

        private void ShowProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            var productListWindow = new ProductListWindow();
            productListWindow.ShowDialog();
        }

        private void ShowUnitsBtn_Click(object sender, RoutedEventArgs e)
        {
            var measureUnitListWindow = new MeasureUnitListWindow();
            measureUnitListWindow.ShowDialog();
        }

        private void ShowGroupsBtn_Click(object sender, RoutedEventArgs e)
        {
            var groupsListWindow = new GroupsListWindow();
            groupsListWindow.ShowDialog();
        }

        private void ShowStatusesBtn_Click(object sender, RoutedEventArgs e)
        {
            var statusListWindow = new StatusListWindow();
            statusListWindow.ShowDialog();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.Frame.GoBack();
        }
    }
}
