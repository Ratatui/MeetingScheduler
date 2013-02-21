namespace MeetingScheduler
{
    using MeetingScheduler.Views;
    using MeetingScheduler.Web;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// <see cref="Page"/> class to present information about the current application.
    /// </summary>
    public partial class ResourceView : Page
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ResourceView"/> class.
        /// </summary>
        public ResourceView()
        {
            InitializeComponent();           
            this.Title = ApplicationStrings.ResourcePageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void resourceDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void ClickAddResource(object sender, System.Windows.RoutedEventArgs e)
        {
            var form = new ResourceForm();
            form.Title = "Add resource";
            form.Show();
            form.Closed += (s, a) =>
                {
                    if (form.DialogResult == true)
                    {
                        resourceDomainDataSource.DataView.Add(form.CurrentResource);
                        resourceDomainDataSource.SubmitChanges();
                    }
                };
        }

        private void ClickEditResource(object sender, System.Windows.RoutedEventArgs e)
        {
            var form = new ResourceForm(resourceDomainDataSource.DataView.CurrentItem as Resource);
            form.Title = "Edit resource";
            form.Show();
            form.Closed += (s, a) =>
            {
                if (form.DialogResult == true)
                {
                    resourceDomainDataSource.SubmitChanges();
                }
            };
        }

        private void ClickDeleteResource(object sender, System.Windows.RoutedEventArgs e)
        {
            resourceDomainDataSource.DataView.Remove(resourceDomainDataSource.DataView.CurrentItem as Resource);
            resourceDomainDataSource.SubmitChanges();
        }
    }
}