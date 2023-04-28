using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Controls
{
    public class TimeSpanPicker : HorizontalStackLayout
    {
        public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(TimeSpanPicker), defaultBindingMode: BindingMode.TwoWay);
        public TimeSpan? Time 
        {
            get => (TimeSpan?)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }
        private Entry _hours;
        private int _hoursValue = 0;
        private int _minutesValue = 0;
        private int _secondsValue = 0;
        private Entry _minutes;
        private Entry _seconds;
        private Label _separator1;
        private Label _separator2;

        public TimeSpanPicker()
        {
            _hours = new Entry()
            {
                FontSize = 40,
                FontFamily = "NunitoBold",
                FlowDirection = FlowDirection.RightToLeft,
                MaxLength = 3,
                Keyboard = Keyboard.Numeric,
                Placeholder = "00"
            };
            _minutes = new Entry()
            {
                FontSize = 40,
                FontFamily = "NunitoBold",
                FlowDirection = FlowDirection.RightToLeft,
                MaxLength = 3,
                Keyboard = Keyboard.Numeric,
                Placeholder = "00"
            };
            _seconds = new Entry()
            {
                FontSize = 40,
                FontFamily = "NunitoBold",
                FlowDirection = FlowDirection.RightToLeft,
                MaxLength = 3,
                Keyboard = Keyboard.Numeric,
                Placeholder = "00"
            };

            _separator1 = new Label()
            {
                Margin = new Thickness()
                {
                    Top = 7
                },
                FontSize = 40,
                Text = ":"
            };

            _separator2 = new Label()
            {
                Margin = new Thickness()
                {
                    Top = 7
                },
                FontSize = 40,
                Text = ":"
            };

            Spacing = 2;
            _hours.TextChanged += Hours_TextChanged;
            _minutes.TextChanged += Minutes_TextChanged;
            _seconds.TextChanged += Seconds_TextChanged;
            Add(_hours);
            Add(_separator1);
            Add(_minutes);
            Add(_separator2);
            Add(_seconds);
        }

        private void Seconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                _secondsValue = 0;
                Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
                return;
            }
            if (!uint.TryParse(e.NewTextValue, out var seconds) || seconds == 0 || seconds > 59)
            {
                _seconds.Text = "";
                _secondsValue = 0;
                Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
                return;
            }

            var value = e.NewTextValue.PadLeft(3, '0').Substring(1, 2);

            _seconds.Text = value;
            _secondsValue = (int)seconds;
            _seconds.CursorPosition = 3;
            Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
        }

        private void Minutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                _minutesValue = 0;
                Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
                return;
            }
            if (!uint.TryParse(e.NewTextValue, out var minutes) || minutes == 0 || minutes > 59)
            {
                _minutes.Text = "";
                _minutesValue = 0;
                Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
                return;
            }

            var value = e.NewTextValue.PadLeft(3, '0').Substring(1, 2);

            _minutes.Text = value;
            _minutesValue = (int)minutes;
            _minutes.CursorPosition = 3;
            Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
        }

        private void Hours_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                _hoursValue = 0;
                Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
                return;
            }
            if (!uint.TryParse(e.NewTextValue, out var hours) || hours == 0)
            {
                _hours.Text = "";
                _hoursValue = 0;
                Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
                return;
            }

            var value = e.NewTextValue.PadLeft(3, '0').Substring(1, 2);

            _hours.Text = value;
            _hoursValue = (int)hours;
            _hours.CursorPosition = 3;
            Time = new TimeSpan(_hoursValue, _minutesValue, _secondsValue);
        }
    }
}
