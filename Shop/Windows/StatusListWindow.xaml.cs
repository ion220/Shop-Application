using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для StatusListWindow.xaml
    /// </summary>
    public partial class StatusListWindow : Window
    {
        public StatusListWindow()
        {
            InitializeComponent();

            DataTable orderStatus = API.SelectAllFromTable("Order_status");

            if (orderStatus != null)
            {
                StatusDataGrid.ItemsSource = orderStatus.DefaultView;
            }
        }
    }
}
