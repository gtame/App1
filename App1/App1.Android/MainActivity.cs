using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using App1.Services;

namespace App1.Droid
{
    [Activity(Label = "App1", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static long DISCONNECT_TIMEOUT = 300000; // 5 min = 5 * 60 * 1000 ms

        private static IApplicationHandler appHandler;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);



            disconnectHandler = new Handler(new MyHandlerICallback(this));
            var app = new App();
            appHandler = (IApplicationHandler)app;
            LoadApplication(app);
        }


        public Handler disconnectHandler;
        public class MyHandlerICallback : Java.Lang.Object, Handler.ICallback
        {
            private MainActivity mainActivity;

            public MyHandlerICallback(MainActivity mainActivity)
            {
                this.mainActivity = mainActivity;
            }

            public bool HandleMessage(Message msg)
            {
                //ToDo
                return true;
            }
        }

        System.Action action = () =>
        {
            // Perform any required operation on disconnect
            appHandler.Logout();
            
        };

        public void resetDisconnectTimer()
        {
            disconnectHandler.RemoveCallbacks(action);
            disconnectHandler.PostDelayed(action, DISCONNECT_TIMEOUT);
        }

        public void stopDisconnectTimer()
        {
            disconnectHandler.RemoveCallbacks(action);
        }

        protected override void OnStop()
        {
            base.OnStop();
            stopDisconnectTimer();
        }

        protected override void OnResume()
        {
            base.OnResume();
            resetDisconnectTimer();
        }

        public override void OnUserInteraction()
        {
            resetDisconnectTimer();
        }
    }
}