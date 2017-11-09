using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Płeć")]
        public GenderType Gender { get; set; }

        public enum GenderType
        {
            Male = 0,
            Female = 1
        }

        [Required]
        [Display(Name = "Data urodzenia")]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<Client_Group> Client_Groups { get; set; }
    }
}