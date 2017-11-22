using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Grupa")]
        public string Name { get; set; }

        public virtual ICollection<Client_Group> Client_Groups { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}