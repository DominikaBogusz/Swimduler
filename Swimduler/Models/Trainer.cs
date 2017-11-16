using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Trainer : Person
    {
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}