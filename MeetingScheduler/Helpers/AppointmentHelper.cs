using Telerik.Windows.Controls.ScheduleView;

namespace MeetingScheduler.Helpers
{
    public static class AppointmentHelper
    {
        public static bool IsRecurring(this IAppointment app)
        {
            return app.RecurrenceRule != null;
        }
    }
}
