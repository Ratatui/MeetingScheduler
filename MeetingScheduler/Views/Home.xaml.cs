namespace MeetingScheduler
{
    using MeetingScheduler.Web;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Telerik.Windows.Controls.ScheduleView;
    using System.Linq;
    using System;
    using Telerik.Windows.Controls;
    using Telerik.Windows.Controls.ScheduleView.ICalendar;
    using MeetingScheduler.Helpers;
    using MeetingScheduler.Views;
    using System.ServiceModel.DomainServices.Client;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
        private SchedulerDomainContext domainContext;
        private RadComboBox SelectResource;

        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
            this.domainContext = new SchedulerDomainContext();
            this.SelectResource = (Application.Current.RootVisual as MainPage).SelectResource;
            this.LoadResources();

            //Scheduler.AppointmentCreated  += Scheduler_AppointmentCreated;
            Scheduler.AppointmentEdited   += Scheduler_AppointmentEdited;
            Scheduler.AppointmentDeleted  += Scheduler_AppointmentDeleted;
            Scheduler.AppointmentCreating += Scheduler_AppointmentCreating;

            SelectResource.SelectionChanged += SelectResource_SelectionChanged;
            this.SelectResource.Visibility = System.Windows.Visibility.Visible;
        }      

        public void SelectResource_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            
                var selectItem = e.AddedItems[0] as RadComboBoxItem;
                this.LoadAppointments(selectItem.Content.ToString());
           
        }

        void Scheduler_AppointmentCreating(object sender, AppointmentCreatingEventArgs e)
        {
            if (WebContext.Current.User.IsAuthenticated)
            {
                var app = new Appointment();
                app.CopyFrom(e.Appointment); 
                e.Cancel = true;
                domainContext.Load<UserTeam>(domainContext.GetTeamByNameQuery(WebContext.Current.User.Name)).Completed += (s, a) =>
                    {
                        domainContext.Load<MeetingScheduler.Web.Resource>(domainContext.GeResourceByIDQuery(domainContext.UserTeams.ElementAt(0).Team)).Completed += (sen, args) =>
                            {

                                LoadOperation loadOp = sen as LoadOperation;
                                var res = loadOp.Entities.ElementAt(0) as MeetingScheduler.Web.Resource;

                               //

                                Telerik.Windows.Controls.Resource resource = new Telerik.Windows.Controls.Resource();
                                resource.ResourceName = res.Name;
                                resource.DisplayName = res.DisplayName;
                                resource.ResourceType = res.ResourceTypes.Name;
                                app.Resources.Add(resource);

                                resource = new Telerik.Windows.Controls.Resource();
                                resource.ResourceName = (SelectResource.SelectedValue as RadComboBoxItem).Content.ToString();
                                resource.DisplayName = (SelectResource.SelectedValue as RadComboBoxItem).Content.ToString();
                                resource.ResourceType = "Room";
                                
                                app.Resources.Add(resource);
                                app.TimeMarker = new TimeMarker();

                                Scheduler_AppointmentCreated(app);

                                RadScheduleViewCommands.EditAppointment.Execute(app, Scheduler);                                                      
                            };
                    };
            }
            else
                e.Cancel = true;
        }

        void Scheduler_AppointmentEdited(object sender, AppointmentEditedEventArgs e)
        {
            var editApp = e.Appointment as Appointment;

            SqlAppointment sqlAppointmentToEdit = domainContext.SqlAppointments.Single(a => a.Id.ToString().Equals(editApp.UniqueId));
            sqlAppointmentToEdit.Start = editApp.Start;
            sqlAppointmentToEdit.End = editApp.End;
            sqlAppointmentToEdit.Subject = editApp.Subject;
            sqlAppointmentToEdit.Body = editApp.Body;
            sqlAppointmentToEdit.Location = editApp.Location;
            sqlAppointmentToEdit.IsAllDayEvent = editApp.IsAllDayEvent;
            sqlAppointmentToEdit.Url = editApp.Url;
            sqlAppointmentToEdit.Importance = editApp.Importance.ToString();            
            sqlAppointmentToEdit.TimeMarker = editApp.TimeMarker.ToString();

            // set the RecurrencePattern and reset the exceptions (delete them) if necessary
            if (editApp.IsRecurring())
            {
                sqlAppointmentToEdit.RecurrencePattern = RecurrencePatternHelper.RecurrencePatternToString(editApp.RecurrenceRule.Pattern);

                if (editApp.RecurrenceRule.Exceptions.Count() == 0)
                {
                    foreach (var exception in sqlAppointmentToEdit.SqlAppointments1)
                    {
                        this.domainContext.SqlAppointments.Remove(exception);
                    }
                }
            }
            else
            {
                // first delete all cascading exception appointments (if any) 
                foreach (var exception in sqlAppointmentToEdit.SqlAppointments1)
                {
                    this.domainContext.SqlAppointments.Remove(exception);
                }
                sqlAppointmentToEdit.RecurrencePattern = null;
            }

            // Add new resources
            foreach (Telerik.Windows.Controls.Resource resource in editApp.Resources)
            {
                if (sqlAppointmentToEdit.AppointmentResources.Count(ar => ar.Resource.Name == resource.ResourceName) == 0)
                {
                    AppointmentResource appResource = new AppointmentResource();
                    appResource.Id = -1;
                    appResource.SqlAppointments = sqlAppointmentToEdit;
                    appResource.Resource = this.domainContext.Resources.Single(r => r.Name == resource.ResourceName);
                    sqlAppointmentToEdit.AppointmentResources.Add(appResource);

                    if (appResource.Resource.ResourceTypes.Name == "Team")
                        editApp.Category = CategoryHelper.MakeCategory(appResource.Resource.Color);
                }
            }
            // Remove old resources
            foreach (AppointmentResource appointmentResource in sqlAppointmentToEdit.AppointmentResources)
            {
                if (editApp.Resources.Count(r => r.ResourceName == appointmentResource.Resource.Name) == 0)
                {
                    this.domainContext.AppointmentResources.Remove(appointmentResource);
                }
            }

            domainContext.SubmitChanges();
        }

        void Scheduler_AppointmentDeleted(object sender, AppointmentDeletedEventArgs e)
        {
            var deletedAppointment = e.Appointment as Appointment;
            var sqlAppointmentToDelete = this.domainContext.SqlAppointments.SingleOrDefault(a => a.Id.ToString().Equals(deletedAppointment.UniqueId));

            foreach (var exception in sqlAppointmentToDelete.SqlAppointments1)
            {
                this.domainContext.SqlAppointments.Remove(exception);
            }

            this.domainContext.SqlAppointments.Remove(sqlAppointmentToDelete);
            this.domainContext.SubmitChanges();
        }

        void Scheduler_AppointmentCreated(Appointment addedAppointment)
        {         
            var sqlAppointment = new SqlAppointment
            {
                Id = new Guid(addedAppointment.UniqueId),
                Start = addedAppointment.Start,
                End = addedAppointment.End,
                Subject = addedAppointment.Subject,
                Body = addedAppointment.Body,
                IsAllDayEvent = addedAppointment.IsAllDayEvent,
                Location = addedAppointment.Location,
                Url = addedAppointment.Url,
                Type = (int)AppointmentType.Regular,                
                TimeZoneString = addedAppointment.TimeZone.ToString(),
                Importance = addedAppointment.Importance.ToString(),
                RecurrencePattern = addedAppointment.IsRecurring() ?
                                    RecurrencePatternHelper.RecurrencePatternToString(addedAppointment.RecurrenceRule.Pattern) :
                                    null,
            };

            foreach (var resource in addedAppointment.Resources)
            {
                var appResource = new AppointmentResource();
                appResource.Id = -1;
                appResource.SqlAppointments = sqlAppointment;
                appResource.Resource = this.domainContext.Resources.Single(r => r.Name == resource.ResourceName);
                sqlAppointment.AppointmentResources.Add(appResource);

                if (appResource.Resource.ResourceTypes.Name == "Team")
                    addedAppointment.Category = CategoryHelper.MakeCategory(appResource.Resource.Color);
            }            
            this.domainContext.SqlAppointments.Add(sqlAppointment);
            (this.Scheduler.AppointmentsSource as ObservableCollection<Appointment>).Add(addedAppointment);
            //this.domainContext.SubmitChanges();
        }

        private void LoadResources()
		{
            try
            {
                SelectResource.Items.Clear();
            }
            catch 
            {
                ;
            }
            var loadResourcesOperation = this.domainContext.Load<MeetingScheduler.Web.Resource>(this.domainContext.GetResourceQuery());
            loadResourcesOperation.Completed += (sender, args) =>
            {
                var typesSource = new ObservableCollection<Telerik.Windows.Controls.ResourceType>();
                this.Scheduler.ResourceTypesSource = null;
                foreach (MeetingScheduler.Web.ResourceType dbResourceType in domainContext.ResourceTypes)
		        {
			        Telerik.Windows.Controls.ResourceType resourceType = new Telerik.Windows.Controls.ResourceType();
			        resourceType.DisplayName = dbResourceType.DisplayName;
			        resourceType.Name = dbResourceType.Name;
			        resourceType.AllowMultipleSelection = dbResourceType.AllowMultipleSelection.Value;                                       

			        foreach (MeetingScheduler.Web.Resource dbResource in dbResourceType.Resource)
			        {
				        Telerik.Windows.Controls.Resource resource = new Telerik.Windows.Controls.Resource();
				        resource.DisplayName = dbResource.DisplayName;
				        resource.ResourceName = dbResource.Name;
				        resourceType.Resources.Add(resource);
                        if (resourceType.DisplayName == "Room")
                        {
                            var item = new RadComboBoxItem();
                            item.Content = dbResource.DisplayName;
                            SelectResource.Items.Add(item);
                        }
                    }
                    if (resourceType.Name == "Room")
                        continue;
                    typesSource.Add(resourceType);
                }
                if (this.SelectResource.Items.Count > 0)
                    this.SelectResource.SelectedIndex = 0;
                this.Scheduler.ResourceTypesSource = typesSource;                
            };
        }

        private void LoadAppointments(string selectResource)
        {
            var tempApps = new ObservableCollection<Appointment>();

            var appointmentsLoadOperation = this.domainContext.Load<SqlAppointment>(this.domainContext.GetSqlAppointmentsQuery());
            appointmentsLoadOperation.Completed += (s, a) =>
            {
                foreach (SqlAppointment sqlAppointment in this.domainContext.SqlAppointments)
                {
                    if (sqlAppointment.AppointmentResources.Count(k => k.Resource.DisplayName == selectResource) == 0)
                        continue;
                    Appointment newApp = new Appointment();
                    newApp.Start = sqlAppointment.Start;
                    newApp.End = sqlAppointment.End;
                    newApp.Subject = sqlAppointment.Subject;
                    newApp.Body = sqlAppointment.Body;
                    newApp.IsAllDayEvent = sqlAppointment.IsAllDayEvent;
                    newApp.Location = sqlAppointment.Location;
                    newApp.Url = sqlAppointment.Url;
                    newApp.UniqueId = sqlAppointment.Id.ToString();

                    newApp.Category = new Category(sqlAppointment.Category, null);
                    newApp.TimeMarker = new TimeMarker(sqlAppointment.TimeMarker, null);
                    newApp.Importance = ImportanceHelper.CreateImportanceFromString(sqlAppointment.Importance);

                    foreach (var appointmentResource in sqlAppointment.AppointmentResources.ToList())
                    {
                        Telerik.Windows.Controls.Resource resource = new Telerik.Windows.Controls.Resource();
                        resource.ResourceName = appointmentResource.Resource.Name;
                        resource.DisplayName = appointmentResource.Resource.DisplayName;
                        resource.ResourceType = appointmentResource.Resource.ResourceTypes.Name;

                        if (appointmentResource.Resource.ResourceTypes.Name == "Team")
                            newApp.Category = CategoryHelper.MakeCategory(appointmentResource.Resource.Color);

                        newApp.Resources.Add(resource);                       
                    }

                    var pattern = new RecurrencePattern();
                    if (RecurrencePatternHelper.TryParseRecurrencePattern(sqlAppointment.RecurrencePattern, out pattern))
                    {
                        newApp.RecurrenceRule = new RecurrenceRule(pattern);
                        foreach (var exception in sqlAppointment.SqlAppointments1)
                        {
                            if (exception.Type == (int)AppointmentType.ExceptionDate)
                            {
                                newApp.RecurrenceRule.AddException(exception.ExceptionDate.Value);
                            }
                            else
                            {
                                var exceptionAppointment = new Appointment()
                                {
                                    Start = exception.Start,
                                    End = exception.End,
                                    Subject = exception.Subject,
                                    Body = exception.Body,
                                    IsAllDayEvent = exception.IsAllDayEvent,
                                    UniqueId = exception.Id.ToString(),
                                    Importance = ImportanceHelper.CreateImportanceFromString(exception.Importance),
                                    TimeMarker = new TimeMarker(exception.TimeMarker, null),
                                    Category =  new Category(exception.Category, null)
                                };
                                newApp.RecurrenceRule.AddException(exception.ExceptionDate.Value, exceptionAppointment);
                            }
                        }
                    }

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