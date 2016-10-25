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
    public class Reviews
    {
        public async Task<ReviewViewModel> GetReview(int id, bool approvedOnly = true)
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

        public async Task<ReviewViewModel> Create(ReviewBindingModel review)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = ctx.Reviews.Create();

                entity.UserGuid = Guid.NewGuid();

                entity.Rating = review.Rating;
                entity.Comment = review.Comment;

                entity.CreateDate = DateTime.Now;
                ctx.Reviews.Add(entity);
                await ctx.SaveChangesAsync();

                return new ReviewViewModel(entity);
            }
        }

        public async Task<ReviewViewModel> Update(int id, ReviewBindingModel review)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                entity.UserGuid = Guid.NewGuid();

                entity.Rating = review.Rating;
                entity.CreateDate = DateTime.Now;
                await ctx.SaveChangesAsync();

                return new ReviewViewModel(entity);
            }
        }

        public async Task Remove(int id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                ctx.Reviews.Remove(entity);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Approve(int id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                entity.Approved = true;
                await ctx.SaveChangesAsync();
            }
        }
   
        public async Task Reject(int id)
        {
            using (var ctx = new CMMIContext())
            {
                var entity = await ctx.Reviews.FindAsync(id);

                if (entity == null) throw new NotFoundException("Review not found.");

                entity.Approved = false;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
