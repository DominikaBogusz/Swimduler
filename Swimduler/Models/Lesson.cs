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

        [EnumDataType(typeof(LessonCycle))]
        [Display(Name = "Cykl")]
        public LessonCycle Cycle { get; set; }

        public enum LessonCycle
        {
            Tygodniowy = 0,
            Dwutygodniowy = 1,
            Miesięczny = 2
        }

        [Required]
        [EnumDataType(typeof(LessonStatus))]
        [Display(Name = "Status")]
        public LessonStatus Status { get; set; }

        public enum LessonStatus
        {
            Zaplanowana = 0,
            Rozpoczęta = 1,
            Zakończona = 2,
            Odwołana = 3
        }

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}