using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IAlertHelper
    {
        Task ShowError(string text);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    }
}
