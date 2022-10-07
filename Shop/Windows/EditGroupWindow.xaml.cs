using SynesthesiaM;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Shop.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditGroupWindow.xaml
    /// </summary>
    public partial class EditGroupWindow : Window
    {
        private int _editableGroupId;
        private List<Group> _groups;

        public EditGroupWindow()
        {
            _editableGroupId = 0;

            InitializeComponent();
            UpdateContext();
            UpdateUI();

            ShowGroupsSelector(false);
        }

        public EditGroupWindow(int groupId)
        {
            _editableGroupId = groupId;

            InitializeComponent();
            UpdateUI();

            TitleText.Text = "Редактирование группы";

            DisplayGroupWithId(_editableGroupId);
        }

        private void UpdateUI()
        {
            UpdateContext();
            GroupsTreeView.ItemsSource = _groups;
        }

        private void UpdateContext()
        {
            _groups = GroupProcessor.GetHierarchicalGroupList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(Validate() == false)
                return;

            bool hasParentGroup = HasParentGroupCheckBox.IsChecked == true;

            string name = NameTextBox.Text;
            int parentGroupId = 0;

            if (hasParentGroup)
            {
                Group selectedGroup = GroupsTreeView.SelectedItem as Group;
                parentGroupId = selectedGroup.Id;
            }
                

            if (_editableGroupId >= 1)
            {
                if (hasParentGroup)
                {
                    if (parentGroupId == _editableGroupId)
                    {
                        MessageBox.Show("Группа не может иметь подгруппой саму себя.");
                        return;
                    }
                    API.UpdateGroup(_editableGroupId, name, parentGroupId);
                }
                else
                {
                    API.UpdateGroup(_editableGroupId, name);
                }
            }
            else
            {
                if (hasParentGroup)
                {
                    API.InsertIntoGroup(name, parentGroupId);
                }
                else
                {
                    API.InsertIntoGroup(name);
                }
            }

            MessageBox.Show("Сохранение успешно.");

            this.Close();
        }

        private bool Validate()
        {
            if (NameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Поле имени не заполнено.");
                return false;
            }

            if (NameTextBox.Text.Length > 50)
            {
                MessageBox.Show("Имя слишком длинное. Максимальная длинна - 50 символов");
                return false;
            }

            if (HasParentGroupCheckBox.IsChecked == true)
            {
                if (GroupsTreeView.SelectedItem is null)
                {
                    MessageBox.Show("Группа не выбрана.");
                    return false;
                }
            }
            return true;
        }

        private void HasMasterGroupCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            if (HasParentGroupCheckBox.IsChecked == true)
            {
                ShowGroupsSelector(true);
            }
            else
            {
                ShowGroupsSelector(false);
            }
        }

        private void ShowGroupsSelector(bool IsActive)
        {
            if (IsActive == true)
            {
                GroupsTreeView.IsEnabled = true;
                TreeViewTitleTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                GroupsTreeView.IsEnabled = false;
                TreeViewTitleTextBlock.Foreground = new SolidColorBrush(Colors.LightSlateGray);
            }
        }

        private void DisplayGroupWithId(int id)
        {
            var gruopRow = API.SelectRowFromTable(id, "Product_group");

            NameTextBox.Text = gruopRow["name"].ToString();

            var selectedGroupParentId = gruopRow["parent_group_id"];

            if (!(selectedGroupParentId is DBNull))
            {
                HasParentGroupCheckBox.IsChecked = true;
                ShowGroupsSelector(true);

                string path = GroupProcessor.GetGroupPathById((int)selectedGroupParentId, _groups);
                GroupsTreeView.SetSelectedItem(path, '/');

            }
            else
            {
                ShowGroupsSelector(false);
            }
        }
    }
}
