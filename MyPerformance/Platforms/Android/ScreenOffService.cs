using Android.App;
using Android.Content;
using AndroidNet = Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Net;

namespace MyPerformance.Platforms.Android
{
    [Service(Label = nameof(ScreenOffService))]
    public class ScreenOffService : Service
    {
        private const string NOTIFICATION_CHANNEL_ID = "1000";
        private const int NOTIFICATION_ID = 1;
        private const string NOTIFICATION_CHANNEL_NAME = "notification";
        private readonly ScreenOffBroadcastReceiver _screenOffBroadcastReceiver;

        public ScreenOffService()
        {
            _screenOffBroadcastReceiver = new ScreenOffBroadcastReceiver();
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICE")
            {
                RegisterNotification();
            }
            else if (intent.Action == "STOP_SERVICE")
            {
                StopForeground(true);
                StopSelfResult(startId);
            }

            return StartCommandResult.Sticky;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterScreenOffBroadcastReceiver();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterScreenOffBroadcastReceiver();
            _screenOffBroadcastReceiver?.Dispose();
        }

        public void Start()
        {
            Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(ScreenOffService));
            startService.SetAction("START_SERVICE");
            MainActivity.ActivityCurrent.StartService(startService);
        }

        private void RegisterScreenOffBroadcastReceiver()
        {
            var filter = new IntentFilter();
            filter.AddAction(Intent.ActionScreenOff);
            RegisterReceiver(_screenOffBroadcastReceiver, filter);
        }

        private void UnregisterScreenOffBroadcastReceiver()
        {
            try
            {
                if (_screenOffBroadcastReceiver != null)
                {
                    UnregisterReceiver(_screenOffBroadcastReceiver);
                }
            }
            catch (Java.Lang.IllegalArgumentException ex)
            {
                Console.WriteLine($"Error while unregistering {nameof(ScreenOffBroadcastReceiver)}. {ex}");
            }
        }

        public void Stop()
        {
            Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
            stopIntent.SetAction("STOP_SERVICE");
            MainActivity.ActivityCurrent.StartService(stopIntent);
        }

        private void CreateNotificationChannel(NotificationManager notificationMnaManager)
        {
            var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
            notificationMnaManager.CreateNotificationChannel(channel);
        }

        private void RegisterNotification()
        {
            var notifcationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateNotificationChannel(notifcationManager);
            }

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notification.SetAutoCancel(false);
            notification.SetOngoing(true);
            notification.SetSmallIcon(Resource.Drawable.ic_clock_black_24dp);
            notification.SetContentTitle("MyPerformance");
            notification.SetContentText("Запущен таймер для выступления...");

            StartForeground(NOTIFICATION_ID, notification.Build());
        }
    }
}
