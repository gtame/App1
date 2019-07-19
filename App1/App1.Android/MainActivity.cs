using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using App1.Services;

using Com.Datalogic.Device;
using Com.Datalogic.Decode;
using Com.Datalogic.Device.Configuration;
using Com.Datalogic.Decode.Configuration;
using Android.Util;

namespace App1.Droid
{
    [Activity(Label = "App1", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IReadListener
    {

        public static long DISCONNECT_TIMEOUT = 300000; // 5 min = 5 * 60 * 1000 ms
        private readonly string LOGTAG = typeof(MainActivity).Name;

        BarcodeManager decoder = null;
        private static IApplicationHandler appHandler;
        private static ICodeReader codeReaderHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);



            disconnectHandler = new Handler(new MyHandlerICallback(this));
            var app = new App();
            appHandler = (IApplicationHandler)app;
            codeReaderHandler = (ICodeReader)app;
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


            Log.Info(LOGTAG, "OnResume");

            // If the decoder instance is null, create it.
            if (decoder == null)
            {
                // Remember an onPause call will set it to null.
                decoder = new BarcodeManager();
            }

            // From here on, we want to be notified with exceptions in case of errors.
            ErrorManager.EnableExceptions(true);

            try
            {
                // add our class as a listener
                decoder.AddReadListener(this);
            }
            catch (DecodeException e)
            {
                Log.Error(LOGTAG, "Error while trying to bind a listener to BarcodeManager", e);
            }
        }

        public override void OnUserInteraction()
        {
            resetDisconnectTimer();
        }


        protected override void OnPause()
        {
            base.OnPause();

            Log.Info(LOGTAG, "onPause");

            // If we have an instance of BarcodeManager.
            if (decoder != null)
            {
                try
                {
                    // Unregister our listener from it and free resources
                    decoder.RemoveReadListener(this);
                }
                catch (Exception e)
                {
                    Log.Error(LOGTAG, "Error while trying to remove a listener from BarcodeManager", e);
                }
            }
        }

        void IReadListener.OnRead(IDecodeResult decodeResult)
        {
            // Change the displayed text to the current received result.
            codeReaderHandler.OnReadCodeBar(decodeResult.Text);
        }
    }
}