using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CMMI.Business;
using CMMI.Business.Models;
using CMMI.Business.Types;

namespace CMMI.Web.Controllers
{
    [Authorize]
    public class ReviewsController : BaseApiController
    {
        private readonly Reviews _reviews;
        public ReviewsController()
        {
            _reviews = new Reviews();
        }

        [AllowAnonymous]
        [Route("api/reviews/{reviewId}"), HttpGet]
        // GET: api/Reviews/5
        public async Task<IHttpActionResult> GetReview(ApiId reviewId)
        {
            var review = await _reviews.GetReview(reviewId);

            return Ok(review);
        }

        [AllowAnonymous]
        [Route("api/users/{userGuid}/reviews"), HttpGet]
        // GET: api/Reviews/5
        public async Task<IHttpActionResult> GetUserReviews(Guid userGuid)
        {
            var review = await _reviews.GetUserReviews(userGuid);

            return Ok(review);
        }

        [AllowAnonymous]
        [Route("api/users/current/reviews"), HttpGet]
        // GET: api/Reviews/5
        public async Task<IHttpActionResult> GetUserReviews()
        {
            return await GetUserReviews(CurrentUserGuid);
        }

        [AllowAnonymous]
        [Route("api/restaurants/{restaurantId}/reviews"), HttpGet]
        // GET: api/Reviews/5
        public async Task<IHttpActionResult> GetRestaurantReviews(ApiId restaurantId)
        {
            var reviews = await _reviews.GetRestaurantReviews(restaurantId);

            return Ok(reviews);
        }

        [Route("api/restaurants/{restaurantId}/reviews"), HttpPost]
        // POST: api/Reviews
        public async Task<IHttpActionResult> Post(ApiId restaurantId, [FromBody]ReviewBindingModel review)
        {
            var created = await _reviews.Create(restaurantId, review);
            return Created("api/reviews/" + created.Id, created);
        }

        [Route("api/reviews/{reviewId}"), HttpPut]
        // PUT: api/Reviews/5
        public async Task<IHttpActionResult> Put(ApiId reviewId, [FromBody]ReviewBindingModel review)
        {
            var result = await _reviews.Update(reviewId, review);
            return Ok(result);
        }

        [Route("api/reviews/{reviewId}"), HttpDelete]
        // DELETE: api/Restaurant/5
        public async Task<IHttpActionResult> Delete(ApiId reviewId)
        {
            await _reviews.Remove(reviewId);
            return StatusCode(HttpStatusCode.Gone);
        }

        [Route("api/reviews/{reviewId}/approve"), HttpPost]
        // POST: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Approve(ApiId reviewId)
        {
            await _reviews.Approve(reviewId);
            return Ok();
        }

        [Route("api/reviews/{reviewId}/reject"), HttpPost]
        // POST: api/Restaurant/5/reject
        public async Task<IHttpActionResult> Reject(ApiId reviewId)
        {
            await _reviews.Reject(reviewId);
            return Ok();
        }
    }
}
