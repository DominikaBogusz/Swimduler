using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Lesson
    {
        [Key]
        [Display(Name = "Rozpoczęcie")]
        public DateTime Beginning { get; set; }

        [Required]
        [Display(Name = "Czas trwania")]
        public TimeSpan Duration { get; set; }

        [Display(Name = "Cykl")]
        public CalendarTimeRange LessonCycle { get; set; }

        [Required]
        [EnumDataType(typeof(LessonStatus))]
        [Display(Name = "Status")]
        public LessonStatus Status { get; set; }

        public enum LessonStatus
        {
            Planned = 0,
            Started = 1,
            Completed = 2,
            Canceled = 3
        }

        [Required]
        public int GroupId { get; set; }
        public virtual Client Group { get; set; }
    }
}