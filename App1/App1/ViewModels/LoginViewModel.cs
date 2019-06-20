using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        public  Action SuccessLogin;
        public Action DisplayInvalidLoginPrompt; 
        private string username;
        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public async void OnSubmit()
        {
            bool isLogged = await DataStore.LogonAsync(UserName, Password);
            if (!isLogged)
            {
                DisplayInvalidLoginPrompt();
            }
            else
            {
                SuccessLogin();
            }
        }

        public async void ExecuteAsync(Action action)
        {
            await Task.Run(action);
        }
    }
}
