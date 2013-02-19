using System;
using System.Windows.Media;

namespace MeetingScheduler.Helpers
{
    public class ColorHelper
    {
        internal static Color CreateColorFromString(string colorString)
        {
            Type t = typeof(Colors);
            Color colorToReturn = (Color)t.GetProperty(colorString).GetValue(null, null);
            
            return colorToReturn;
        }
    }
}
