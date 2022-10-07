using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
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
using System.Xml.Linq;

namespace Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        private DataRowView _editableOrder;

        private DataTable _orderProducts;
        
        public EditOrderWindow()
        {
            InitializeComponent();
            InitStatusesCombobox();

            int? lastNumber = API.GetLastOrderNumber();
            if (lastNumber is null)
                NumberTextBlock.Text = "1000000";
            else
                NumberTextBlock.Text = ((lastNumber) + 1).ToString();

            CreationDateTextBlock.Text = "Auto";
        }

        public EditOrderWindow(DataRowView editableOrder)
        {
            InitializeComponent();
            InitStatusesCombobox();

            _editableOrder = editableOrder;

            TitleText.Text = "Редактирование заказа";

            DispalyEditableOrder(editableOrder);
        }

        private void InitStatusesCombobox()
        {
            var statuses = API.SelectAllFromTable("Order_status");
            StatusComboBox.ItemsSource = statuses.DefaultView;
            StatusComboBox.SelectedItem = StatusComboBox.Items[0];
        }

        private void UpdateContext()
        {
            foreach (DataRow product in _orderProducts.Rows)
            {
                var amount = product["amount"];
                var price = product["price"];

                var priceSum = new SqlMoney((int)amount * float.Parse(price.ToString()));

                product["priceSum"] = priceSum;
            }
        }

        private void UpdateUI()
        {
            UpdateContext();

            SqlMoney finalPrice = 0;

            foreach (DataRow product in _orderProducts.Rows)
            {
                finalPrice += (SqlMoney)product["priceSum"];
            }
            

            ProductDataGrid.ItemsSource = _orderProducts.DefaultView;
            FinalPriceTextBlock.Text = "Итого:\n" + finalPrice;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!Validate())
                return;

            if (TitleText.Text == "Редактирование заказа")
                UpdateOrder(_editableOrder);
            else
                CreateNewOrder();

            MessageBox.Show("Сохранение успешно.");

            this.Close();
        }

        private void UpdateOrder(DataRowView editableOrder)
        {
            int id = (int)editableOrder["id"];

            string number = editableOrder["number"].ToString();

            DateTime creationDateTime = (DateTime)(editableOrder["creation_datetime"]);

            DateTime deliveryDateTime = (DateTime)(DeliveryDatePicker.SelectedDate + GetTime());

            string address = AddressTextBox.Text.Trim();

            string phoneNumber = PhoneTextBox.Text.Trim();

            int statusId = (int)(StatusComboBox.SelectionBoxItem as DataRowView)["id"];

            string cardNumber = CardNumberTextBox.Text.Trim();

            string comment = CommentTextBox.Text.Trim();

            API.UpdateOrder(id, number, creationDateTime, deliveryDateTime, address, phoneNumber, statusId, cardNumber, comment);

            API.DeleteOrderProductByOrderId(id);

            foreach (DataRow product in _orderProducts.Rows)
            {
                API.InsertIntoOrderProduct(API.GetLastOrderId(), (int)product["id"], (int)product["amount"]);
            }
        }

        private void CreateNewOrder()
        {
            string number = NumberTextBlock.Text;

            DateTime creationDateTime = DateTime.Now;

            DateTime deliveryDateTime = (DateTime)(DeliveryDatePicker.SelectedDate + GetTime());

            string address = AddressTextBox.Text.Trim();

            string phoneNumber = PhoneTextBox.Text.Trim();

            int statusId = (int)(StatusComboBox.SelectionBoxItem as DataRowView)["id"];

            string cardNumber = CardNumberTextBox.Text.Trim();

            string comment = CommentTextBox.Text.Trim();

            API.InsertIntoOrder(number, creationDateTime, deliveryDateTime, address, phoneNumber, statusId, cardNumber, comment);

            foreach (DataRow product in _orderProducts.Rows)
            {
                API.InsertIntoOrderProduct(API.GetLastOrderId(), (int)product["id"], (int)product["amount"]);
            }
        }

        private TimeSpan GetTime()
        {
            int hours = Int32.Parse(HoursTextBox.Text);

            int minutes = Int32.Parse(MinutesTextBox.Text);

            TimeSpan time = new TimeSpan(0, hours, minutes, 0);

            return time;
        }

        private bool Validate()
        {
            if (!ValidateText())
            {
                MessageBox.Show("Обязательные поля должны быть заполнены.");
                return false;
            }

            if (!ValidateDateTime())
            {
                MessageBox.Show(@"Некорректный ввод даты\времени доствки.");
                return false;
            }

            if (_orderProducts is null)
            {
                MessageBox.Show("Товары не выбраны.");
                return false;
            }

            if (_orderProducts.Rows.Count == 0)
            {
                MessageBox.Show("Товары не выбраны.");
                return false;
            }

            return true;
        }

        private bool ValidateText()
        {
            if (HoursTextBox.Text.Length == 0 || MinutesTextBox.Text.Length == 0)
                return false;

            if (AddressTextBox.Text.Length == 0)
                return false;

            if (PhoneTextBox.Text.Length == 0)
                return false;

            return true;
        }

        private bool ValidateDateTime()
        {
            if (DeliveryDatePicker.SelectedDate is null)
                return false;

            if (Int32.TryParse(HoursTextBox.Text, out var hour) && Int32.TryParse(MinutesTextBox.Text, out var minute))
            {
                if (hour >= 0 || hour <= 24 || minute >= 0 || minute <= 60)
                    return true;
                return false;
            }

            return false;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var productListWindow = new ProductListWindow(true);
            productListWindow.ShowDialog();

            var selectedProduct = productListWindow.SelectedProduct;

            if (selectedProduct is null)
                return;

            var selectedProductRow = selectedProduct.Rows[0];

            InitOrderProductsTable(selectedProduct);

            if (!(_orderProducts.Rows.Cast<DataRow>()
                    .FirstOrDefault(row => (int)row["id"] == (int)selectedProductRow["id"]) is null))
            {
                MessageBox.Show("Данный товар уже добавлен.");
                return;
            }
            
            _orderProducts.ImportRow(selectedProductRow);

            _orderProducts.Rows[_orderProducts.Rows.Count - 1]["amount"] = 1;

            UpdateUI();
        }

        private void InitOrderProductsTable(DataTable productTable)
        {
            if (_orderProducts is null)
                _orderProducts = productTable.Clone();
            if (!_orderProducts.Columns.Contains("amount"))
                _orderProducts.Columns.Add("amount", typeof(int));
            if (!_orderProducts.Columns.Contains("priceSum"))
                _orderProducts.Columns.Add("priceSum", typeof(SqlMoney));
        }

        private void ChangeAmountButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is null)
                return;

            if (AmountTextBox.Text.Length == 0)
                return;

            if (!Int32.TryParse(AmountTextBox.Text.Trim(), out var amount))
                return;

            if (amount < 1)
                return;

            var selectedItemId = (int)(ProductDataGrid.SelectedItem as DataRowView)["id"];

            ChangeLocalProductAmount(selectedItemId, amount);

            UpdateUI();
        }

        private void ChangeLocalProductAmount(int productId, int amount)
        {
            foreach (DataRow product in _orderProducts.Rows)
            {
                if ((int)product["id"] == productId)
                {
                    product["amount"] = amount;
                    break;
                }
            }
        }

        private void DeleteProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is null)
                return;

            var selectedItemId = (ProductDataGrid.SelectedItem as DataRowView)["id"];

            foreach (DataRow product in _orderProducts.Rows)
            {
                if ((int)product["id"] == (int)selectedItemId)
                {
                    _orderProducts.Rows.Remove(product);
                    break;
                }
            }
            UpdateUI();
        }

        private void DispalyEditableOrder(DataRowView editableOrder)
        {
            NumberTextBlock.Text = editableOrder["number"].ToString();

            var creationDateTime = (DateTime)editableOrder["creation_datetime"];
            CreationDateTextBlock.Text = creationDateTime.ToString("yyyy-MM-dd HH:mm");

            var deliveryDateTime = (DateTime)editableOrder["creation_datetime"];
            DeliveryDatePicker.SelectedDate = deliveryDateTime.Date;
            HoursTextBox.Text = deliveryDateTime.Hour.ToString();
            MinutesTextBox.Text = deliveryDateTime.Minute.ToString();

            AddressTextBox.Text = editableOrder["client_address"].ToString();
            
            PhoneTextBox.Text = editableOrder["client_phone"].ToString();

            var selectedStatusRow = API.GetOrderStatus((int)editableOrder["id"]).DefaultView[0];
            SetSelectedComboboxItem((int)selectedStatusRow["id"]);

            CardNumberTextBox.Text = editableOrder["card_number"].ToString();

            CommentTextBox.Text = editableOrder["comment"].ToString();

            if (CardNumberTextBox.Text.Contains("Не указано"))
                CardNumberTextBox.Text = null;

            if (CommentTextBox.Text.Contains("Не указано"))
                CommentTextBox.Text = null;

            var selectedProducts = API.GetProductsByOrderId((int)editableOrder["id"]);

            InitOrderProductsTable(selectedProducts);
            _orderProducts = ProductListWindow.GetProductForView(selectedProducts);
            InitOrderProductsTable(selectedProducts);

            UpdateUI(); 
        }

        private void SetSelectedComboboxItem(int id)
        {
            StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<DataRowView>().Single(row => (int)row["id"] == id);
        }
    }
}
