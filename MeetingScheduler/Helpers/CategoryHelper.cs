using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MeetingScheduler.Helpers
{
    public class CategoryHelper
    {
        public static Category MakeCategory(String color)
        {
            color = color.Trim();
            var Col = new Color();
            switch (color)
            {
                case "Red":
                    Col = Colors.Red; break;
                case "Green":
                    Col = Colors.Green; break;
                case "Yellow":
                    Col = Colors.Yellow; break;
                case "Brown":
                    Col = Colors.Brown; break;
                case "Orange":
                    Col = Colors.Orange; break;
                case "Purple":
                    Col = Colors.Purple; break;
                case "Cyan":
                    Col = Colors.Cyan; break;
                case "Magenta":
                    Col = Colors.Magenta; break;
                case "Gray":
                    Col = Colors.Gray; break;
                default:
                    break;
            }
            return new Category(color, new SolidColorBrush(Col));
        }
    }
}
