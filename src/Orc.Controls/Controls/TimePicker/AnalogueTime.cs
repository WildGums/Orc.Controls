namespace Orc.Controls
{
    using System;
    using Orc.Controls.Enums;

    public struct AnalogueTime : IComparable<AnalogueTime>, IEquatable<AnalogueTime>
    {
        public AnalogueTime(int hour, int minute, Meridiem meridiem)
        {
            if (hour < 0 || hour > 12)
            {
                throw new ArgumentException("Hour should be in [0..12]", nameof(hour));
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentException("Minute should be in [0..59]", nameof(minute));
            }

            Hour = hour;
            Minute = minute;
            Meridiem = meridiem;
        }

        public AnalogueTime(TimeSpan time)
        {
            Minute = time.Minutes;

            if (time.Hours == 0)
            {
                // 12:00 AM to 12:59 AM
                Hour = time.Hours + 12;
                Meridiem = Meridiem.AM;
            }
            else if (time.Hours <= 11)
            {
                // 01:00 AM to 11:59 AM
                Hour = time.Hours;
                Meridiem = Meridiem.AM;
            }
            else if (time.Hours == 12)
            {
                // 12:00PM to 12:59 PM
                Hour = time.Hours;
                Meridiem = Meridiem.PM;
            }
            else if (time.Hours < 24)
            {
                // 01:00PM to 11:59 PM
                Hour = time.Hours - 12;
                Meridiem = Meridiem.PM;
            }
            else
            {
                throw new ArgumentException("Cannot create a time from a TimeSpan that represents more than 24 hours", nameof(time));
            }
        }

        public AnalogueTime(DigitalTime time)
            : this(time.ToTimeSpan()) { }

        public int Hour { get; }
        public int Minute { get; }

        public Meridiem Meridiem { get; }

        public TimeSpan ToTimeSpan()
        {
            if (Meridiem == Meridiem.AM)
            {
                if (Hour == 12)
                {
                    // 12:00 AM to 12:59 AM is 00:00 to 00:59
                    return new TimeSpan(Hour - 12, Minute, 0);
                }
                else
                {
                    // 01:00 AM to 11:59 AM is 01:00 to 11:59
                    return new TimeSpan(Hour, Minute, 0);
                }
            }
            else
            {
                if (Hour == 12)
                {
                    // 12:00 PM to 12:59 PM is 12:00 to 12:59
                    return new TimeSpan(Hour, Minute, 0);
                }
                else
                {
                    // 01:00 PM to 11:59 PM is 13:00 to 23:59 
                    return new TimeSpan(Hour + 12, Minute, 0);
                }
            }
        }

        public DateTime ToDateTime()
        {
            if (Meridiem == Meridiem.AM)
            {
                if (Hour == 12)
                {
                    // 12:00 AM to 12:59 AM is 00:00 to 00:59
                    return new DateTime(0000, 00, 00, Hour - 12, Minute, 0);
                }
                else
                {
                    // 01:00 AM to 11:59 AM is 01:00 to 11:59
                    return new DateTime(0000, 00, 00, Hour, Minute, 0);
                }
            }
            else
            {
                if (Hour == 12)
                {
                    // 12:00 PM to 12:59 PM is 12:00 to 12:59
                    return new DateTime(0000, 00, 00, Hour, Minute, 0);
                }
                else
                {
                    // 01:00 PM to 11:59 PM is 13:00 to 23:59 
                    return new DateTime(0000, 00, 00, Hour + 12, Minute, 0);
                }
            }
        }

        public DigitalTime ToDigitalTime()
        {
            return new DigitalTime(ToTimeSpan());
        }

        public override int GetHashCode()
        {
            return ((int)Meridiem * 1000) + Hour * 100 + Minute;
        }

        public override bool Equals(object obj)
        {
            return obj != null &&
                   obj is AnalogueTime &&
                   Equals((AnalogueTime)obj);
        }

        public bool Equals(AnalogueTime other)
        {
            return other.Hour == Hour &&
                   other.Minute == Minute &&
                   other.Meridiem == Meridiem;
        }

        public override string ToString()
        {
            var dt = DateTime.Today.Add(ToTimeSpan());
            return dt.ToString("HH:mm (tt)");
        }

        public int CompareTo(AnalogueTime other)
        {
            return ToTimeSpan().CompareTo(other.ToTimeSpan());
        }

        public static bool operator <(AnalogueTime left, AnalogueTime right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(AnalogueTime left, AnalogueTime right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator ==(AnalogueTime left, AnalogueTime right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AnalogueTime left, AnalogueTime right)
        {
            return !Equals(left, right);
        }
    }
}

