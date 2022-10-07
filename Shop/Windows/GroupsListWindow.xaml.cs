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
    /// Логика взаимодействия для GroupsListWindow.xaml
    /// </summary>
    public partial class GroupsListWindow : Window
    {
        private List<Group> _groups;

        public GroupsListWindow()
        {
            InitializeComponent();
            UpdateContext();
            UpdateUI();
        }

        private void UpdateContext()
        {
            _groups = GroupProcessor.GetHierarchicalGroupList();
        }

        private void UpdateUI()
        {
            GroupsTreeView.ItemsSource = _groups;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection())
                return;

            Group group = GroupsTreeView.SelectedItem as Group;

            Window editGroupWindow = new EditGroupWindow(group.Id);
            editGroupWindow.ShowDialog();
            UpdateContext();
            UpdateUI();

        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(!ValidateSelection())
                return;

            var DialogResult = MessageBox.Show("Выбранная группа и все вложенные группы" +
                                               " будут безвозвратно удалены.",
                "Предупреждение",
                MessageBoxButton.OKCancel);

            if (DialogResult == MessageBoxResult.Cancel)
                return;

            var selectedItem = GroupsTreeView.SelectedItem as Group;
            
            DeleteGroup(selectedItem);

            UpdateContext();

            UpdateUI();
        }

        private void DeleteGroup(Group groupForDelete)
        {
            API.DeleteFromTable(groupForDelete.Id, "Product_group");

            if(groupForDelete.Groups is null)
                return;

            foreach (var group in groupForDelete.Groups)
            {
                DeleteGroup(group);
            }
        }

        private bool ValidateSelection()
        {
            if (GroupsTreeView.SelectedItem is null)
            {
                MessageBox.Show("Группа не выбрана.");
                return false;
            }

            return true;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Window editGroupWindow = new EditGroupWindow();
            editGroupWindow.ShowDialog();
            UpdateContext();
            UpdateUI();
        }

        private void ExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Content.ToString().Equals("Развернуть главные группы"))
            {
                SetExpaandState(true);

                btn.Content = "Свернуть главные группы";
            }
            else
            {
                SetExpaandState(false);

                btn.Content = "Развернуть главные группы";
            }
        }

        private void SetExpaandState(bool state)
        {
            foreach (var item in GroupsTreeView.ItemContainerGenerator.Items)
            {
                var tvi = GroupsTreeView.ItemContainerGenerator.ContainerFromItem(item)
                    as TreeViewItem;
                tvi.IsExpanded = state;
            }
        }
    }
}
