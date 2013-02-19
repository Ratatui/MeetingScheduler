using System;
using Telerik.Windows.Controls.ScheduleView;

namespace MeetingScheduler.Helpers
{
    public class ImportanceHelper
    {
        internal static Importance CreateImportanceFromString(string importanceString)
        {
            Importance result;
            if (Enum.TryParse<Importance>(importanceString, out result))
            {
                return result;
            }
            else
            {
                return Importance.None;
            }
        }
    }
}
