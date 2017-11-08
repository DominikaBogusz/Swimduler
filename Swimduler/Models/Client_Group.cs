using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swimduler.Models
{
    public class Client_Group
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}