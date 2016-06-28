﻿using System;

namespace Nikse.SubtitleEdit.PluginLogic
{
    internal class TimeCode
    {
        private TimeSpan _time;
        public const double BaseUnit = 1000.0; // Base unit of time

        public static double ParseToMilliseconds(string text)
        {
            var parts = text.Split(new[] { ':', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                int hours;
                int minutes;
                int seconds;
                int milliseconds;
                if (int.TryParse(parts[0], out hours) && int.TryParse(parts[1], out minutes) && int.TryParse(parts[2], out seconds) && int.TryParse(parts[3], out milliseconds))
                {
                    TimeSpan ts = new TimeSpan(0, hours, minutes, seconds, milliseconds);
                    return ts.TotalMilliseconds;
                }
            }
            return 0;
        }

        public TimeCode(double milliseconds)
        {
            TimeSpan = TimeSpan.FromMilliseconds(milliseconds);
        }

        public TimeCode(TimeSpan ts)
        {
            TimeSpan = ts;
        }

        public TimeCode(int hour, int minute, int seconds, int milliseconds)
        {
            _time = new TimeSpan(0, hour, minute, seconds, milliseconds);
        }

        public int Hours
        {
            get { return _time.Hours; }
            set { _time = new TimeSpan(0, value, _time.Minutes, _time.Seconds, _time.Milliseconds); }
        }

        public int Minutes
        {
            get { return _time.Minutes; }
            set { _time = new TimeSpan(0, _time.Hours, value, _time.Seconds, _time.Milliseconds); }
        }

        public int Seconds
        {
            get { return _time.Seconds; }
            set { _time = new TimeSpan(0, _time.Hours, _time.Minutes, value, _time.Milliseconds); }
        }

        public int Milliseconds
        {
            get { return _time.Milliseconds; }
            set { _time = new TimeSpan(0, _time.Hours, _time.Minutes, _time.Seconds, value); }
        }

        public double TotalMilliseconds
        {
            get { return _time.TotalMilliseconds; }
            set { _time = TimeSpan.FromMilliseconds(value); }
        }

        public double TotalSeconds
        {
            get { return _time.TotalSeconds; }
            set { _time = TimeSpan.FromSeconds(value); }
        }

        public TimeSpan TimeSpan
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        public void AddTime(int hour, int minutes, int seconds, int milliseconds)
        {
            Hours += hour;
            Minutes += minutes;
            Seconds += seconds;
            Milliseconds += milliseconds;
        }

        public void AddTime(long milliseconds)
        {
            _time = TimeSpan.FromMilliseconds(_time.TotalMilliseconds + milliseconds);
        }

        public void AddTime(TimeSpan timeSpan)
        {
            _time = TimeSpan.FromMilliseconds(_time.TotalMilliseconds + timeSpan.TotalMilliseconds);
        }

        public void AddTime(double milliseconds)
        {
            _time = TimeSpan.FromMilliseconds(_time.TotalMilliseconds + milliseconds);
        }

        public override string ToString()
        {
            var s = $"{_time.Hours:00}:{_time.Minutes:00}:{_time.Seconds:00},{_time.Milliseconds:000}";
            if (TotalMilliseconds < 0)
                s = "-" + s.Replace("-", string.Empty);
            return s;
        }

    }
}