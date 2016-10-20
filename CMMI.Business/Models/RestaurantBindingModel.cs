﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMI.Business.Models
{
    public class RestaurantBindingModel
    {
        [Required(ErrorMessage = "City must be specified."), MaxLength(100, ErrorMessage = "Name must be less than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "City must be specified."), MaxLength(100, ErrorMessage = "City must be less than 100 characters.")]
        public string City { get; set; }
    }
}