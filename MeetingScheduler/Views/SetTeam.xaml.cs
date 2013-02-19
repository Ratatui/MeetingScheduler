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
    public partial class SetTeam : ChildWindow
    {
        private SchedulerDomainContext domainContext;

        public SetTeam()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamCB.SelectedIndex == -1)
                return;

            UserTeam ut = new UserTeam();
            ut.Team = Convert.ToInt16((TeamCB.Items[TeamCB.SelectedIndex] as ComboBoxItem).Tag);
            ut.User = WebContext.Current.User.Name;

            domainContext.UserTeams.Add(ut);
            domainContext.SubmitChanges();
            this.DialogResult = true;
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            domainContext = new SchedulerDomainContext();
            domainContext.Load<Resource>(domainContext.GetResourceQuery()).Completed += (s, args) =>
                {
                    foreach (var Res in domainContext.Resources)
                    {
                        if (Res.ResourceTypes.DisplayName == "Team")
                        {                         
                            TeamCB.Items.Add(new ComboBoxItem()
                                {
                                    Content = Res.DisplayName,
                                    Tag = Res.Id
                                }); 
                        }
                    }
                };
        }
    }
}

