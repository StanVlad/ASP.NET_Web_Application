using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_DAW
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int categoryId { get; set; }

        [Required(ErrorMessage = "Acest câmp este obligatoriu")]
        public string name { get; set; }

        [Required(ErrorMessage = "Acest câmp este obligatoriu")]
        public string description { get; set; }
    }
}