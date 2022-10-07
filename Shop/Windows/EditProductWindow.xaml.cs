using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
using SynesthesiaM;

namespace Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private List<Group> _groups;

        private int _editableProductId;

        public EditProductWindow()
        {
            InitializeComponent();
            UpdateContext();
            UpdateUI();
            InitUnitsComboBox();

            _editableProductId = 0;
        }

        public EditProductWindow(int productId)
        {
            InitializeComponent();
            UpdateContext();
            UpdateUI();
            InitUnitsComboBox();

            _editableProductId = productId;

            TitleText.Text = "Редактирование продукта";
            DisplayProductWithId(productId);
        }

        private void UpdateContext()
        {
            _groups = GroupProcessor.GetHierarchicalGroupList();

        }

        private void UpdateUI()
        {
            GroupsTreeView.ItemsSource = _groups;
        }

        private void InitUnitsComboBox()
        {
            var measureUnitTable = API.SelectAllFromTable("Measure_unit");
            UnitComboBox.ItemsSource = measureUnitTable.DefaultView;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            
            string name = NameTextBox.Text;
            string description = DescriptionTextBox.Text;

            string textPrice = PriceTextBox.Text;
            float price = TryParsePrice(textPrice);

            if (price == 0) return;
            
            var selectedUnit = UnitComboBox.SelectedItem as DataRowView;
            int unitId = (int)selectedUnit["id"];

            Group selectedGroup = GroupsTreeView.SelectedItem as Group; 
            int groupId = selectedGroup.Id;

            bool result;
            if (_editableProductId >= 1)
            {
                result = API.UpdateProduct(_editableProductId, name, description, unitId, groupId, price);

            }
            else
            {
                result = API.InsertIntoProduct(name, description, unitId, groupId, price);
            }

            if (result)
                MessageBox.Show("Сохранение успешно.");

            this.Close();
        }

        private bool Validate()
        {
            if (NameTextBox.Text.Length == 0 ||
                DescriptionTextBox.Text.Length == 0 ||
                PriceTextBox.Text.Length == 0)
            {
                MessageBox.Show("Заполните все поля.");
                return false;
            }

            if (UnitComboBox.SelectedItem is null ||
                GroupsTreeView.SelectedItem is null)
            {
                MessageBox.Show("Выберите еденицу измерения и группу.");
                return false;
            }

            return true;
        }

        private float TryParsePrice(string textPrice)
        {
            float price;

            if (!float.TryParse(textPrice, out price))
            {
                var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
                if (!float.TryParse(textPrice, NumberStyles.Float, cultureInfo, out price))
                {
                    MessageBox.Show("Некорректный ввод цены.");
                    return 0;
                }
            }

            if (price <= 0)
            {
                MessageBox.Show("Цена не может быть 0 или меньше");
                return 0;
            }

            return price;
        }

        private void DisplayProductWithId(int id)
        {
            var productRow = API.SelectRowFromTable(id, "Product");

            NameTextBox.Text = productRow["name"].ToString();
            DescriptionTextBox.Text = productRow["description"].ToString();
            PriceTextBox.Text = productRow["price"].ToString();

            SetSelectedComboboxItem((int)productRow["measure_unit_id"]);

            string path = GroupProcessor.GetGroupPathById((int)productRow["group_id"], _groups);

            GroupsTreeView.SetSelectedItem(path, '/');
        }

        private void SetSelectedComboboxItem(int id)
        {
            UnitComboBox.SelectedItem = UnitComboBox.Items.Cast<DataRowView>().Single(row => (int)row["id"] == id);
        }
    }
}
