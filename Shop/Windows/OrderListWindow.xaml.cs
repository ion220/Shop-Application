using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderListWindow.xaml
    /// </summary>
    public partial class OrderListWindow : Window
    {
        private DataTable _orders;

        public OrderListWindow()
        {
            InitializeComponent();
            UpdateContext();
            UpdateUI();
        }
        private void UpdateUI()
        {
            OrdersList.ItemsSource = _orders.DefaultView;
        }

        private void UpdateUI(string findText)
        {
            DataTable sortOrders = _orders.Clone();

            foreach (DataRow row in _orders.Rows)
            {
                if (row["id"].ToString().Contains(findText))
                {
                    sortOrders.ImportRow(row);
                }
                else if (row["number"].ToString().Contains(findText))
                {
                    sortOrders.ImportRow(row);
                }
                else if (row["client_phone"].ToString().Contains(findText))
                {
                    sortOrders.ImportRow(row);
                }
                else if (Convert.ToDateTime(row["creation_datetime"]).ToString("yyyy-MM-dd HH:mm").Contains(findText))
                {
                    sortOrders.ImportRow(row);
                }
            }

            OrdersList.ItemsSource = sortOrders.DefaultView;
        }

        private void UpdateContext()
        {
            _orders = GetOrdersForView();
        }

        public static DataTable GetOrdersForView()
        {
            DataTable orders = API.SelectAllFromTable("[Order]");

            if(orders is null) return null;

            var statuses = API.SelectAllFromTable("Order_status");
            var products = API.SelectAllFromTable("Product");
            var orderProducts = API.SelectAllFromTable("Order_Product");

            DataTable ordersViewTable = orders.Clone();

            ordersViewTable.Columns[6].DataType = typeof(string);
            ordersViewTable.Columns.Add(new DataColumn("products", typeof(string)));

            for (int i = 0; i < orders.Rows.Count; i++)
            {
                DataRow order = orders.Rows[i];

                ordersViewTable.ImportRow(order);

                foreach (DataRow status in statuses.Rows)
                {
                    if ((int)status["id"] == (int)order["status_id"])
                    {
                        ordersViewTable.Rows[i]["status_id"] = status["name"].ToString();
                        break;
                    }
                }

                string productsInOrder = "";

                foreach (DataRow orderProduct in orderProducts.Rows)
                {
                    if ((int)orderProduct["id_order"] == (int)order["id"])
                    {
                        foreach (DataRow product in products.Rows)
                        {
                            if ((int)product["id"] == (int)orderProduct["id_product"])
                                productsInOrder += product["name"] + ", ";
                        }
                    }
                }

                ordersViewTable.Rows[i]["products"] = productsInOrder.TrimEnd(' ', ',');

                if (ordersViewTable.Rows[i]["card_number"].ToString().Trim().Equals(""))
                    ordersViewTable.Rows[i]["card_number"] = "Не указано";
                
                if (ordersViewTable.Rows[i]["comment"].ToString().Trim().Equals(""))
                    ordersViewTable.Rows[i]["comment"] = "Не указано";
                
                if (ordersViewTable.Rows[i]["products"].ToString().Trim().Equals(""))
                    ordersViewTable.Rows[i]["products"] = "Не указано";
            }

            return ordersViewTable;
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string findText = FindTextBox.Text.Trim();
            if (findText.Length == 0)
                return;
            
            UpdateUI(findText);
        }

        private void FindTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (FindTextBox.Text.Trim().Length == 0)
            {
                UpdateUI();
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var editOrderWindow = new EditOrderWindow();
            editOrderWindow.ShowDialog();

            UpdateContext();
            UpdateUI();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelectedOrder())
                return;

            var editOrderWindow = new EditOrderWindow(OrdersList.SelectedItem as DataRowView);
            editOrderWindow.Show();
            
            UpdateContext();
            UpdateUI();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidateSelectedOrder())
                return;

            var DialogResult = MessageBox.Show("Заказ будет безвозвратно удален.",
                "Предупреждение",
                MessageBoxButton.OKCancel);

            if (DialogResult == MessageBoxResult.Cancel)
                return;

            int selectedOrderId = (int)(OrdersList.SelectedItem as DataRowView)["id"];

            API.DeleteOrderProductByOrderId(selectedOrderId);

            API.DeleteFromTable(selectedOrderId, "[Order]");

            MessageBox.Show("Заказ успешно удален.");

            UpdateContext();
            UpdateUI();
        }

        private bool ValidateSelectedOrder()
        {
            if (OrdersList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Элемент не выбран.");
                return false;
            }

            return true;
        }
    }
}
