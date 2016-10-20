using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMMI.Data;

namespace CMMI.Business.Models
{
    public class ReviewViewModel
    {
        public ReviewViewModel(Review review)
        {
            Id = review.Id;
            Rating = review.Rating;
            Comment = review.Comment;
            CreateDate = review.CreateDate;
            User = new UserBaseViewModel
            {
                UserGuid = review.UserGuid,
                Name = ""
            };
        }

        public int Id { get; set; }
        public short Rating { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreateDate { get; set; }
        public UserBaseViewModel User { get; set; }
    }
}
