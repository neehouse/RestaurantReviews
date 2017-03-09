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
    public class Reviews : IdentityBase
    {
        public async Task<ReviewViewModel> GetReview(long id, bool approvedOnly = true)
        {
            using (var ctx = new CMMIContext())
            {
                var query = ctx.Reviews.Where(x => x.Id == id);

                if (approvedOnly) query = query.Where(x => x.Approved);

                var entity = await query.SingleOrDefaultAsync();

                if (entity == null) throw new NotFoundException("Review not found.");

                return new ReviewViewModel(entity);
            }
        }

        public async Task<IEnumerable<ReviewViewModel>> GetUserReviews(Guid userGuid, bool approvedOnly = true)
        {
            using (var ctx = new CMMIContext())
            {
                var query = ctx.Reviews
                    .Where(x => x.UserGuid == userGuid);

                if (approvedOnly) query = query.Where(x => x.Approved);

                var enties = await query.ToListAsync();

                return enties.Select(x => new ReviewViewModel(x));
            }
        }

        public async Task<IEnumerable<ReviewViewModel>> GetRestaurantReviews(long restaurantId, bool approvedOnly = true)
        {
            using (var ctx = new CMMIContext())
            {
                var query = ctx.Reviews
                    .Where(x => x.RestaurantId == restaurantId);

                if (approvedOnly) query = query.Where(x => x.Approved);

                var enties = await query.ToListAsync();

                return enties.Select(x => new ReviewViewModel(x));
            }
        }

        public async Task<ReviewViewModel> Create(long restaurantId ,ReviewBindingModel review)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = ctx.Reviews.Create();

                entity.UserGuid = CurrentUserGuid;

                entity.RestaurantId = restaurantId;
                entity.Rating = review.Rating;
                entity.Comment = review.Comment;

                entity.CreateDate = DateTime.Now;

                ctx.Reviews.Add(entity);
                await ctx.SaveChangesAsync();

                await UpdateRestaurantReviewDetails(ctx, entity.Restaurant);

                return new ReviewViewModel(entity);
            }
        }

        public async Task<ReviewViewModel> Update(long id, ReviewBindingModel review)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                entity.Rating = review.Rating;
                entity.Comment = review.Comment;

                await ctx.SaveChangesAsync();

                await UpdateRestaurantReviewDetails(ctx, entity.Restaurant);

                return new ReviewViewModel(entity);
            }
        }

        public async Task UpdateRestaurantReviewDetails(CMMIContext ctx, Restaurant restaurant)
        {
            var restaurantReviews = restaurant.Reviews.Where(x => x.Approved).ToList();

            if (restaurantReviews.Any())
            {
                restaurant.Rating = restaurantReviews.Average(x => x.Rating);
                restaurant.ReviewCount = (short) restaurantReviews.Count();

                await ctx.SaveChangesAsync();
            }
        }

        public async Task Remove(long id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                ctx.Reviews.Remove(entity);

                await ctx.SaveChangesAsync();

                await UpdateRestaurantReviewDetails(ctx, entity.Restaurant);
            }
        }

        public async Task Approve(long id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                entity.Approved = true;

                await ctx.SaveChangesAsync();

                await UpdateRestaurantReviewDetails(ctx, entity.Restaurant);
            }
        }
   
        public async Task Reject(long id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                entity.Approved = false;

                await ctx.SaveChangesAsync();

                await UpdateRestaurantReviewDetails(ctx, entity.Restaurant);
            }
        }
    }
}
