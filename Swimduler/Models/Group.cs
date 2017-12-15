﻿using System;
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
        [Display(Name = "Nazwa grupy")]
        public string Name { get; set; }

        [Display(Name = "Powiązane lekcje")]
        public virtual ICollection<Lesson> Lessons { get; set; }

        public virtual ICollection<Client_Group> Client_Groups { get; set; }
    }
}