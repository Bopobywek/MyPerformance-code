using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Platforms.Android
{
    [BroadcastReceiver(Name = "com.example.MyPerformance.ScreenOffBroadcastReceiver", Label = "ScreenOffBroadcastReceiver", Exported = true)]
    [IntentFilter(new[] { Intent.ActionScreenOff }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class ScreenOffBroadcastReceiver : BroadcastReceiver
    {
        private PowerManager.WakeLock _wakeLock;

        public ScreenOffBroadcastReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionScreenOff)
            {
                AcquireWakeLock();
            }
        }

        private void AcquireWakeLock()
        {
            _wakeLock?.Release();

            WakeLockFlags wakeFlags = WakeLockFlags.Partial;

            PowerManager pm = (PowerManager)global::Android.App.Application.Context.GetSystemService(global::Android.Content.Context.PowerService);
            _wakeLock = pm.NewWakeLock(wakeFlags, typeof(ScreenOffBroadcastReceiver).FullName);
            _wakeLock.Acquire();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _wakeLock?.Release();
        }
    }
}
