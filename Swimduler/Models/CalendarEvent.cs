using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Comments { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public string ThemeColor { get; set; }

        public int? LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}