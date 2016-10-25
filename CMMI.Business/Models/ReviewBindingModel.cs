using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMI.Business.Models
{
    public class ReviewBindingModel
    {
        [Required(ErrorMessage = "Rating must be specified.")]
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5.")]
        [DisplayName("Rating")]
        public short Rating { get; set; }

        [MaxLength(2000, ErrorMessage = "Comment must be less than 2000 characters.")]
        [DisplayName("Comment")]
        public string Comment { get; set; }
    }
}
