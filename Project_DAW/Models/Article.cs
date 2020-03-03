using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_DAW
{
     public class Article
     {
         [Key]
         [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
         public int articleId { get; set; }

         [Required(ErrorMessage = "Acest câmp este obligatoriu")]
         public string title { get; set; }

         [Required(ErrorMessage = "Acest câmp este obligatoriu")]
         public string content { get; set; }

         public DateTime date { get; set; }

         public bool last { get; set; } //tells if this version is the last for this article

         public string userId { get; set; } //user who created the article
         public virtual ApplicationUser User { get; set; }
    }
}