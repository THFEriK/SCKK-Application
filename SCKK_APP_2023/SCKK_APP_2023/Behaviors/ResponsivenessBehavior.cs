using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SCKK_APP_2023.Behaviors
{
    internal class ResponsivenessBehavior
    {


        public static int GetIsResponsiveProperty(DependencyObject obj)
        {
            return (int)obj.GetValue(IsResponsivePropertyProperty);
        }

        public static void SetIsResponsiveProperty(DependencyObject obj, int value)
        {
            obj.SetValue(IsResponsivePropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsResponsiveProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsResponsivePropertyProperty =
            DependencyProperty.RegisterAttached("IsResponsiveProperty", typeof(bool), typeof(ResponsivenessBehavior), new PropertyMetadata(false));


    }
}
