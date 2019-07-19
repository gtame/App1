using App1.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class BaseContentPage: ContentPage
    {


        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string>(this, App.EVENT_CODEBAR_READ, OnCodeBarRead);
            //MessagingCenter.Subscribe<string>(this, App.EVENT_CODEBAR_READ);
            //Subscribe CodeBar event
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //Unsubscribe Codebar event
            MessagingCenter.Unsubscribe<string>(this, App.EVENT_CODEBAR_READ);
        }

        protected virtual void OnCodeBarRead(string text)
        {
            if (BindingContext!=null && BindingContext is ICodeReader)
            {
                ((ICodeReader)BindingContext).OnReadCodeBar(text);
            }
        }
    }
}
