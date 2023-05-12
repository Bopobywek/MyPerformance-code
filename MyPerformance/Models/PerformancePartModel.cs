using System;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Models
{
    [Table("performance_parts")]
    public class PerformancePartModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Position { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string Color { get; set; }

        [ForeignKey(typeof(PerformanceModel))]
        public int PerformanceId { get; set; }

        [Ignore]
        public bool IsBeingDragged { get; set; }
        [Ignore]
        public bool IsBeingDraggedOver { get; set; }

    }
}
