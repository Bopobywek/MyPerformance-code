using Android.App;
using Android.Content;
using AndroidIcuUtil = Android.Icu.Util;
using Android.Provider;
using AndroidX.Core.App;
using static Android.Provider.CalendarContract;
using Android.Widget;

namespace MyPerformance.Platforms.Android
{
    public class CalendarEventService
    {
        public static void SetEvent(CalendarEvent calendarEvent)
        {
            if (MainActivity.ActivityCurrent is null)
            {
                return;
            }

            try
            {
                Intent eventValues = new Intent(Intent.ActionInsert);
                eventValues.SetData(CalendarContract.Events.ContentUri);
                eventValues.AddFlags(ActivityFlags.NewTask);
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.Title, calendarEvent.Title);
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.Description, calendarEvent.Description);
                var notifyTime = calendarEvent.BeginTime;
                eventValues.PutExtra(CalendarContract.ExtraEventBeginTime,
                    GetDateTimeInJavaMs(notifyTime.Year, notifyTime.Month, notifyTime.Day, notifyTime.Hour, notifyTime.Minute));
                if (calendarEvent.IsEndTimeSet)
                {
                    notifyTime = calendarEvent.EndTime;
                    eventValues.PutExtra(CalendarContract.ExtraEventEndTime,
                    GetDateTimeInJavaMs(notifyTime.Year, notifyTime.Month, notifyTime.Day, notifyTime.Hour, notifyTime.Minute));
                }
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
                eventValues.PutExtra(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");

                MainActivity.ActivityCurrent.StartActivity(eventValues);
            }
            catch (Java.Lang.Exception)
            {
                Toast.MakeText(MainActivity.ActivityCurrent, "Не удалось создать событие...", ToastLength.Long).Show();
            }
        }

        private static long GetDateTimeInJavaMs(int year, int month, int day, int hr, int min)
        {
            Java.Util.Calendar c = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Java.Util.CalendarField.DayOfMonth, day);
            c.Set(Java.Util.CalendarField.HourOfDay, hr);
            c.Set(Java.Util.CalendarField.Minute, min);
            c.Set(Java.Util.CalendarField.Month, month - 1);
            c.Set(Java.Util.CalendarField.Year, year);

            return c.TimeInMillis;
        }
    }
}
