using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        [ValidateDateRange("1/1/2000", "31/12/2050")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Rozpoczęcie zajęć")]
        public DateTime Beginning { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Czas trwania")]
        public TimeSpan Duration { get; set; }

        [Required]
        [EnumDataType(typeof(LessonCycle))]
        [Display(Name = "Cykl")]
        public LessonCycle Cycle { get; set; }

        public enum LessonCycle
        {
            Brak = 0,
            Tygodniowy = 1,
            Dwutygodniowy = 2,
            Miesięczny = 3
        }

        [DataType(DataType.DateTime)]
        [Display(Name = "Zakończenie cyklu")]
        public DateTime? CycleEnd { get; set; }

        [Display(Name = "Kolor w kalendarzu")]
        public string ThemeColor { get; set; }

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        [Display(Name = "Powiązane wydarzenia")]
        public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }
    }

    public class ValidateDateRange : ValidationAttribute
    {
        public DateTime FirstDate { get; set; }
        public DateTime SecondDate { get; set; }

        public ValidateDateRange(string date1, string date2)
        {
            FirstDate = Convert.ToDateTime(date1);
            SecondDate = Convert.ToDateTime(date2);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = Convert.ToDateTime(value);

            if (date >= FirstDate && date <= SecondDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Data powinna zawierać się w latach od " + FirstDate.Year + " do " + SecondDate.Year + " roku.");
            }
        }
    }
}