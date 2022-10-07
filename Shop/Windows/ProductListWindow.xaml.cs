using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
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
    /// Логика взаимодействия для ProductListWindow.xaml
    /// </summary>
    public partial class ProductListWindow : Window
    {
        private DataTable _products;

        public DataTable SelectedProduct { get; private set; }
        public ProductListWindow()
        {
            InitializeComponent();
            UpdateContextUI();
        }

        public ProductListWindow(bool IsSelector)
        {
            InitializeComponent();
            UpdateContextUI();

            GetButton.Visibility = Visibility.Visible;
            ProductDataGrid.SelectionMode = DataGridSelectionMode.Single;
        }

        public static DataTable GetProductForView(DataTable products)
        {
            if(products is null) return null;

            var groups = API.SelectAllFromTable("Product_group");
            var units = API.SelectAllFromTable("Measure_unit");
            
            DataTable outProducts = products.Clone();

            outProducts.Columns["measure_unit_id"].DataType = typeof(string);
            outProducts.Columns["measure_unit_id"].ColumnName = "measure_unit";
            
            outProducts.Columns["group_id"].DataType = typeof(string);
            outProducts.Columns["group_id"].ColumnName = "group";

            for (int i = 0; i < products.Rows.Count; i++)
            {
                DataRow product = products.Rows[i];

                outProducts.ImportRow(product);

                foreach (DataRow unitRow in units.Rows)
                {
                    if ((int)unitRow["id"] == (int)product["measure_unit_id"])
                    {
                        outProducts.Rows[i]["measure_unit"] = unitRow["short_name"].ToString();
                        break;
                    }
                }

                foreach (DataRow groupRow in groups.Rows)
                {
                    if ((int)groupRow["id"] == (int)product["group_id"])
                    {
                        outProducts.Rows[i]["group"] = groupRow["name"].ToString();
                        break;
                    }
                }
            }
            return outProducts;
        }

        private void UpdateContextUI()
        {
            var products = API.SelectAllFromTable("Product");

            _products = GetProductForView(products);

            ProductDataGrid.ItemsSource = _products.DefaultView;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SelectionValidate())
                return;

            if (ProductDataGrid.SelectedItems.Count > 1)
            {
                MessageBox.Show("Ошибка.\nВыбраны несколько элементов.");
                return;
            }

            int id = (int)(ProductDataGrid.SelectedItem as DataRowView)["id"];

            Window editProductWinoWindow = new EditProductWindow(id);
            editProductWinoWindow.ShowDialog();
            UpdateContextUI();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Window editProductWinoWindow = new EditProductWindow();
            editProductWinoWindow.ShowDialog();
            UpdateContextUI();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SelectionValidate())
                return;

            var DialogResult = MessageBox.Show("Строка будет безвозвратно удалена.",
                "Предупреждение",
                MessageBoxButton.OKCancel);

            if (DialogResult == MessageBoxResult.Cancel)
                return;
            
            foreach (var item in ProductDataGrid.SelectedItems)
            {
                var row = item as DataRowView;

                int id = (int)row["id"];

                API.DeleteFromTable(id, "Product");
            }
            UpdateContextUI();
        }

        private void GetButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SelectionValidate())
                return;

            SelectedProduct = _products.Clone();

            var selectedItem = ProductDataGrid.SelectedItem as DataRowView;

            foreach (DataRow row in _products.Rows)
            {
                if ((int)row["id"] == (int)(selectedItem[0]))
                {
                    SelectedProduct.ImportRow(row);
                    break;
                }
            }

            this.Close();
        }

        private bool SelectionValidate()
        {
            if (ProductDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Элемент не выбран.");
                return false;
            }

            return true;
        }
    }
}
