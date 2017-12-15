using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models.Views
{
    public class GroupsViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa grupy")]
        public string Name { get; set; }

        [Display(Name = "Uczestnicy zajęć")]
        public List<CheckBoxViewModel> Clients { get; set; }
    }
}