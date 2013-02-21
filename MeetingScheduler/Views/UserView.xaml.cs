namespace MeetingScheduler
{
    using MeetingScheduler.Views;
    using MeetingScheduler.Web;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// <see cref="Page"/> class to present information about the current application.
    /// </summary>
    public partial class UserView : Page
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ResourceView"/> class.
        /// </summary>
        public UserView()
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

        private void userteamDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                        userteamDomainDataSource.DataView.Add(form.CurrentResource);
                        userteamDomainDataSource.SubmitChanges();
                    }
                };
        }

        private void ClickEditResource(object sender, System.Windows.RoutedEventArgs e)
        {
            var form = new ResourceForm(userteamDomainDataSource.DataView.CurrentItem as Resource);
            form.Title = "Edit resource";
            form.Show();
            form.Closed += (s, a) =>
            {
                if (form.DialogResult == true)
                {
                    userteamDomainDataSource.SubmitChanges();
                }
            };
        }

        private void ClickDeleteResource(object sender, System.Windows.RoutedEventArgs e)
        {
            userteamDomainDataSource.DataView.Remove(userteamDomainDataSource.DataView.CurrentItem as Resource);
            userteamDomainDataSource.SubmitChanges();
        }
    }
}