using MeetingScheduler.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MeetingScheduler.Views
{
    public partial class ResourceForm : ChildWindow
    {
        public Resource CurrentResource { get; protected set; }

        public ResourceForm()
        {
            InitializeComponent();
            CurrentResource = new Resource();
            grid1.DataContext = CurrentResource;
        }

        public ResourceForm(Resource value)
        {
            InitializeComponent();

            CurrentResource = value;
            grid1.DataContext = CurrentResource;

            typeComboBox.SelectedIndex = CurrentResource.ResourceTypeId - 1;
            typeComboBox.IsEnabled = false;
            if (CurrentResource.ResourceTypes.Name == "Room")
                colorComboBox.IsEnabled = false;
            else
            {
                colorComboBox.IsEnabled = true;
                for (int i = 0; i < colorComboBox.Items.Count; i++)
                    if ((colorComboBox.Items[i] as ComboBoxItem).Content.ToString() == CurrentResource.Color.Trim())
                    {
                        colorComboBox.SelectedIndex = i;
                        break;
                    }
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentResource.ResourceTypeId = Convert.ToInt16((typeComboBox.SelectedItem as ComboBoxItem).Tag.ToString());
            if (CurrentResource.ResourceTypeId == 2)
                CurrentResource.Color = (colorComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void resourceDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems[0] as ComboBoxItem).Content.ToString() == "Room")
            {
                colorComboBox.SelectedIndex = -1;
                colorComboBox.IsEnabled = false;
            }
            else
            {             
                colorComboBox.IsEnabled = true;
                colorComboBox.SelectedIndex = 0;
            }
        }       
    }
}

