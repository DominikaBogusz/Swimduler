using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Nazwisko nie może być dłuższe niż 100 znaków.")]
        [Display(Name = "Nazwisko")]
        public string SecondName { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName;
            }
        }

        [Required]
        [StringLength(100, ErrorMessage = "Nazwa ulicy nie może być dłuższa niż 100 znaków.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Numer mieszkania nie może być dłuższy niz 10 znaków.")]
        [Display(Name = "Numer mieszkania")]
        public string ApartmentNumber { get; set; }

        [Required]
        [RegularExpression(@"\d{2}-\d{3} {1}[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+([ -]*[A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż]*)*", ErrorMessage = "Podaj poprawny kod pocztowy i miasto (np. 37-100 Łańcut).")]
        [Display(Name = "Kod pocztowy i miasto")]
        public string PostCode { get; set; }

        [Display(Name = "Adres")]
        public string Address
        {
            get
            {
                return Street + " " + ApartmentNumber + ", " + PostCode;
            }
        }

        [Required]
        [RegularExpression("[0-9]{9}", ErrorMessage = "Numer telefonu musi składać się z 9 cyfr.")]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }
    }
}