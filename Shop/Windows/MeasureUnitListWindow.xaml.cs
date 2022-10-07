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
    /// Логика взаимодействия для MeasureUnitListWindow.xaml
    /// </summary>
    public partial class MeasureUnitListWindow : Window
    {
        private DataTable _measureUnits;

        public MeasureUnitListWindow()
        {
            InitializeComponent();
            UpdateContextUI();
        }

        private void UpdateContextUI()
        {
            _measureUnits = API.SelectAllFromTable("Measure_unit");

            if (_measureUnits != null)
            {
                UnitsDataGrid.ItemsSource = _measureUnits.DefaultView;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidateText())
                return;

            string name = NameTextBox.Text.Trim();
            string shortName = ShortNameTextBox.Text.Trim();

            if (IdTextBlock.Text.Equals("Auto"))
            {
                API.InsertIntoMeasureUnit(name, shortName);
            }
            else
            {
                int id = Int32.Parse(IdTextBlock.Text);
                API.UpdateMeasureUnit(id, name, shortName);
            }
            
            UpdateContextUI();
        }

        private bool ValidateText()
        {
            if (NameTextBox.Text.Trim().Length == 0 ||
                ShortNameTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Ошибка. Поля не заполнены.");
                return false;
            }

            return true;
        }

        private bool ValidateSelection()
        {
            if (UnitsDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Еденица измерения не выбрана");
                return false;
            }

            return true;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(!ValidateSelection())
                return;

            if (UnitsDataGrid.SelectedItems.Count > 1)
            {
                MessageBox.Show("Ошибка. Выбрано несколько элементов.");
                return;
            }

            DispayUnit(UnitsDataGrid.SelectedItem as DataRowView);

            EditToolsGrid.IsEnabled = true;
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            IdTextBlock.Text = "Auto";
            EditToolsGrid.IsEnabled = true;
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection())
                return;

            var DialogResult = MessageBox.Show("Выбранная еденица измерения будет безвозвратно удалена.",
                "Предупреждение",
                MessageBoxButton.OKCancel);

            if (DialogResult == MessageBoxResult.Cancel)
                return;

            foreach (var item in UnitsDataGrid.SelectedItems)
            {
                var row = item as DataRowView;

                int id = (int)row["id"];

                API.DeleteFromTable(id, "Measure_unit");
            }

            IdTextBlock.Text = "Auto";

            UpdateContextUI();
        }

        private void DispayUnit(DataRowView unit)
        {
            IdTextBlock.Text = unit["id"].ToString();
            NameTextBox.Text = unit["name"].ToString();
            ShortNameTextBox.Text = unit["short_name"].ToString();
        }
    }

    class MeasureUnit
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string ShortName { get; set; }
    }
}
