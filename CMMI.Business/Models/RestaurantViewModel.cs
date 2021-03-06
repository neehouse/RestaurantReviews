﻿using System;
using System.Collections.Generic;
using CMMI.Business.Types;
using CMMI.Data;

namespace CMMI.Business.Models
{
    public class RestaurantCityViewModel
    {
        public string City { get; set; }
        public IEnumerable<RestaurantViewModel> Restaurants { get; set; }
    }

    public class RestaurantViewModel
    {
        public RestaurantViewModel(Restaurant restaurant)
        {
            Id = restaurant.Id;
            Name = restaurant.Name;
            City = restaurant.City;
            Rating = restaurant.Rating;
            ReviewCount = restaurant.ReviewCount;
            CreateDate = restaurant.CreateDate;
            User = new UserBaseViewModel
            {
                UserGuid = restaurant.UserGuid,
                Name = ""
            };
        }

        public ApiId Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreateDate { get; set; }
        public UserBaseViewModel User { get; set; }
    }
}