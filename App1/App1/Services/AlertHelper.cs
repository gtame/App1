using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Services
{
    public class AlertHelper : IAlertHelper
    {
        public   Task  ShowError(string text)
        {
            return Application.Current.MainPage.DisplayAlert("Error", text, "Ok");
        }

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);

        }
    }
}
