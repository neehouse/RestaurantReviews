using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CMMI.Business;
using CMMI.Business.Models;

namespace CMMI.Web.Controllers
{
    [Authorize]
    public class ReviewsController : ApiController
    {
        private readonly Reviews _Reviews;
        public ReviewsController()
        {
            _Reviews = new Reviews();
        }

        [AllowAnonymous]
        [Route("api/review/{id}"), HttpGet]
        // GET: api/Reviews/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var review = await _Reviews.GetReview(id);

            return Ok(review);
        }

        [Route("api/restaurant/{id}/review"), HttpPost]
        // POST: api/Reviews
        public async Task<IHttpActionResult> Post(int id, [FromBody]ReviewBindingModel review)
        {
            if (ModelState.IsValid)
            {
                var created = await _Reviews.Create(review);
                return Created("api/review/" + created.Id, created);
            }
            return BadRequest(ModelState);
        }

        [Route("api/review/{id}"), HttpPut]
        // PUT: api/Reviews/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]ReviewBindingModel review)
        {
            if (ModelState.IsValid)
            {
                var result = await _Reviews.Update(id, review);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Route("api/review/{id}"), HttpDelete]
        // DELETE: api/Restaurant/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _Reviews.Remove(id);
            return StatusCode(HttpStatusCode.Gone);
        }

        [Route("api/review/{id}/approve"), HttpPost]
        // PUT: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Approve(int id)
        {
            await _Reviews.Approve(id);
            return Ok();
        }

        [Route("api/review/{id}/reject"), HttpPost]
        // PUT: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Reject(int id)
        {
            await _Reviews.Reject(id);
            return Ok();
        }
    }
}
