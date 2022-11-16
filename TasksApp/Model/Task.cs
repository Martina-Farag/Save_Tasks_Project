using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksApp.Model
{
    public class Task
    {
        public int taskId { get; set; }
        public string taskProjectSymbol { get; set; }
        public string taskText { get; set; }
        public DateTime? taskStartDateTime { get; set; }
        [NotMapped]public string taskStartDate { get => taskStartDateTime.HasValue==true?taskStartDateTime.Value.ToShortDateString():string.Empty;  }
        [NotMapped]public string taskStartTime { get => taskStartDateTime.HasValue == true ? taskStartDateTime.Value.ToShortTimeString() : string.Empty; }
        public DateTime? taskEndDateTime { get; set; }
        [NotMapped] public string taskEndDate { get => taskEndDateTime.HasValue == true ? taskEndDateTime.Value.ToShortDateString() : string.Empty; }
        [NotMapped] public string taskEndTime { get => taskEndDateTime.HasValue == true ? taskEndDateTime.Value.ToShortTimeString() : string.Empty; }
        public string taskDurationText { get; set; }
        public DateTime? creationDateTime { get; set; }
        public DateTime? lastUpdateDateTime { get; set; }
    }
}
