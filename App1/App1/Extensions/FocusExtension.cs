using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Extensions
{
    public static class FocusExtension
    {

        public static BindableProperty FocusProperty = BindableProperty.CreateAttached("Focus", typeof(bool), typeof(VisualElement),false, propertyChanged: OnIsFocusedPropertyChanged);



        public static bool GetFocus(BindableObject view)
        {
            return (bool)view.GetValue(FocusProperty);
        }

        public static void SetFocus(BindableObject view, bool value)
        {
            view.SetValue(FocusProperty, value);
        }

        static void OnIsFocusedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Property changed implementation goes here
            //if ((bool)newValue)
            //{
                var view = bindable as View;
                if (view == null)
                {
                    return;
                }

                view.Focus();
            //}
        }
    }
}
