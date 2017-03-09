using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMMI.Business.Exceptions;
using CMMI.Business.Models;
using CMMI.Data;

namespace CMMI.Business
{
    public class Restaurants : IdentityBase
    {
        public async Task<RestaurantViewModel> GetRestaurant(long id, bool approvedOnly = false)
        {
            using (var ctx = new CMMIContext())
            {
                var query = ctx.Restaurants.Where(x => x.Id == id);

                if (approvedOnly) query = query.Where(x => x.Approved);

                var entity = await query.SingleOrDefaultAsync();

                if (entity == null) throw new NotFoundException("Restaurant not found.");

                return new RestaurantViewModel(entity);
            }
        }

        //public async Task<IEnumerable<ReviewViewModel>> GetRestaurantReviews(long id, bool approvedOnly = true)
        //{
        //    using (var ctx = new CMMIContext())
        //    {
        //        var entity = await ctx.Restaurants.FindAsync(id);

        //        if (entity == null) throw new NotFoundException("Restaurant not found.");

        //        var query = ctx.Reviews.Where(x => x.RestaurantId == id);

        //        if (approvedOnly) query = query.Where(x => x.Approved);

        //        var results = await query.ToListAsync();

        //        return results.Select(x => new ReviewViewModel(x));
        //    }
        //}

        public async Task<IEnumerable<RestaurantCityViewModel>> GetRestaurantsGroupedByCity(string city, bool approvedOnly = true)
        {
            using (var ctx = new CMMIContext())
            {
                var query = ctx.Restaurants.AsQueryable();

                if (city != null) query = query.Where(x => x.City == city);

                if (approvedOnly) query = query.Where(x => x.Approved);

                var results = await query.OrderBy(x => x.City).ToListAsync();

                return results
                    .GroupBy(x => x.City)
                    .Select(x => new RestaurantCityViewModel
                    {
                        City = x.Key,
                        Restaurants = x.Select(y => new RestaurantViewModel(y))
                    });
            }
        }

        public async Task<IEnumerable<RestaurantViewModel>> GetRestaurantsByCity(string city, bool approvedOnly = true)
        {
            using (var ctx = new CMMIContext())
            {
                var query = ctx.Restaurants.Where(x => x.City == city);

                if (approvedOnly) query = query.Where(x => x.Approved);

                var results = await query.ToListAsync();

                return results.Select(x => new RestaurantViewModel(x));
            }
        }

        public async Task<IEnumerable<string>> GetCityTypeAhead(string search)
        {
            using (var ctx = new CMMIContext())
            {
                return await ctx.Restaurants
                    .Where(x => x.Approved && x.City.StartsWith(search))
                    .Select(x => x.City)
                    .Distinct()
                    .ToArrayAsync();
            }
        }

        public async Task<RestaurantViewModel> Create(RestaurantBindingModel restaurant)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = ctx.Restaurants.Create();
                entity.Name = restaurant.Name;
                entity.City = restaurant.City;

                entity.UserGuid = CurrentUserGuid;

                entity.Rating = 0;
                entity.ReviewCount = 0;
                entity.CreateDate = DateTime.Now;
                ctx.Restaurants.Add(entity);
                await ctx.SaveChangesAsync();

                return new RestaurantViewModel(entity);
            }
        }

        public async Task<RestaurantViewModel> Update(long id, RestaurantBindingModel restaurant)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Restaurants.FindAsync(id);

                if (entity == null) throw new NotFoundException("Restaurant not found.");

                entity.Name = restaurant.Name;
                entity.City = restaurant.City;

                await ctx.SaveChangesAsync();

                return new RestaurantViewModel(entity);
            }
        }

        public async Task Remove(long id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Restaurants.FindAsync(id);

                if (entity == null) throw new NotFoundException("Restaurant not found.");

                ctx.Restaurants.Remove(entity);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Approve(long id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Restaurants.FindAsync(id);

                if (entity == null) throw new NotFoundException("Restaurant not found.");

                entity.Approved = true;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Reject(long id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Restaurants.FindAsync(id);

                if (entity == null) throw new NotFoundException("Restaurant not found.");

                entity.Approved = false;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
