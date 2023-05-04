using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MyPerformance.Models
{
    [Table("performances")]
    public class PerformanceModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsDateSet { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsVibrationEnable { get; set; }
        public bool IsNotificationEnable { get; set; }

        public int NumberOfLaunches { get; set; } = 0;
        public double TotalDuration { get; set; } = 0;
        public double TotalDelayTime { get; set; } = 0;

        [Ignore]
        public TimeSpan AverageDuration
        {
            get
            {
                return TimeSpan.FromSeconds(NumberOfLaunches > 0 ? Math.Round(TotalDuration / NumberOfLaunches) : 0);
            }
        }
        [Ignore]
        public TimeSpan AverageDelayTime
        {
            get
            {
                return TimeSpan.FromSeconds(NumberOfLaunches > 0 ? Math.Round(TotalDelayTime / NumberOfLaunches) : 0);
            }
        }
        [OneToMany("PerformanceId")]
        public PerformancePartModel[] PerformanceParts { get; set; }
    }
}
