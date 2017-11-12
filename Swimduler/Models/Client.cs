using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Client : Person
    {
        [Required]
        [Display(Name = "Płeć")]
        public GenderType Gender { get; set; }

        public enum GenderType
        {
            Mężczyzna = 0,
            Kobieta = 1
        }

        [Required]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1930", "1/1/2015", ErrorMessage = "Wprowadź poprawną datę urodzenia.")]
        [Display(Name = "Data urodzenia")]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<Client_Group> Client_Groups { get; set; }
    }
}