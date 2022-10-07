using System;
using System.Collections.Generic;
using System.Data;
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

namespace Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AssemblyOrderListWindow.xaml
    /// </summary>
    public partial class AssemblingOrderListWindow : Window
    {
        public AssemblingOrderListWindow()
        {
            InitializeComponent();

            var orders = OrderListWindow.GetOrdersForView();

            var sortedRows = orders.Rows.Cast<DataRow>()
                .Where(o => o["status_id"].ToString().Equals("В работе сборщика"));

            var sortedOrders = orders.Clone();

            foreach (DataRow row in sortedRows)
            {
                sortedOrders.ImportRow(row);
            }

            OrdersList.ItemsSource = sortedOrders.DefaultView;
        }
    }
}
