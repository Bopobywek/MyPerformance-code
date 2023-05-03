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
        public DateTime Date { get; set; }
        public bool IsVibrationEnable { get; set; }
        public bool IsNotificationEnable { get; set; }

        [OneToMany("PerformanceId")]
        public PerformancePartModel[] PerformanceParts { get; set; }
    }
}
