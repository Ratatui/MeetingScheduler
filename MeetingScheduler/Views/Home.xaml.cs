namespace MeetingScheduler
{
    using MeetingScheduler.Web;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Telerik.Windows.Controls.ScheduleView;
    using System.Linq;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
        private SchedulerDomainContext domainContext;

        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;

            this.domainContext = new SchedulerDomainContext();

            this.LoadAppointments();
        }

        private void LoadAppointments()
        {
            var tempApps = new ObservableCollection<Appointment>();

            var appointmentsLoadOperation = this.domainContext.Load<SqlAppointment>(this.domainContext.GetSqlAppointmentsQuery());
            appointmentsLoadOperation.Completed += (s, a) =>
            {
                foreach (SqlAppointment sqlAppointment in this.domainContext.SqlAppointments)
                {
                    Appointment newApp = new Appointment();
                    newApp.Start = sqlAppointment.Start;
                    newApp.End = sqlAppointment.End;
                    newApp.Subject = sqlAppointment.Subject;
                    newApp.Body = sqlAppointment.Body;
                    newApp.IsAllDayEvent = sqlAppointment.IsAllDayEvent;
                    newApp.Location = sqlAppointment.Location;
                    newApp.Url = sqlAppointment.Url;
                    newApp.UniqueId = sqlAppointment.Id.ToString();

                    tempApps.Add(newApp);                           
                };
                this.Scheduler.AppointmentsSource = tempApps;
            };
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}