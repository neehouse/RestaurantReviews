using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CMMI.Business;
using CMMI.Business.Models;
using Microsoft.ApplicationInsights.WindowsServer;

namespace CMMI.Web.Controllers
{
    [Authorize]
    public class RestaurantController : ApiController
    {
        private readonly Restaurants _Restaurants;
        public RestaurantController()
        {
            _Restaurants = new Restaurants();
        }

        [AllowAnonymous]
        [Route("api/restaurant"), HttpGet]
        // GET: api/Restaurant
        public async Task<IHttpActionResult> Get(string city)
        {
            var restaurants = await _Restaurants.GetRestaurantsByCity(city);
            return Ok(restaurants);
        }

        [AllowAnonymous]
        [Route("api/restaurant/cities"), HttpGet]
        // GET: api/Restaurant
        public async Task<IHttpActionResult> GetCities(string search)
        {
            var cities = await _Restaurants.GetCityTypeAhead(search);
            return Ok(cities);
        }

        [AllowAnonymous]
        [Route("api/restaurant/{id}"), HttpGet]
        // GET: api/Restaurant/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var restaurant = await _Restaurants.GetRestaurant(id);
            return Ok(restaurant);
        }

        [AllowAnonymous]
        [Route("api/restaurant/{id}/reviews"), HttpGet]
        // GET: api/Restaurant/5/Reviews
        public async Task<IHttpActionResult> GetReviews(int id)
        {
            var restaurant = await _Restaurants.GetRestaurantReviews(id);
            return Ok(restaurant);
        }

        [Route("api/restaurant"), HttpPost]
        // POST: api/Restaurant
        public async Task<IHttpActionResult> Post([FromBody] RestaurantBindingModel restaurant)
        {
            var created = await _Restaurants.Create(restaurant);
            return Created("api/Restaurant/" + created.Id, created);
        }

        [Route("api/restaurant/{id}"), HttpPut]
        // PUT: api/Restaurant/5
        public async Task<IHttpActionResult> Put(int id, [FromBody] RestaurantBindingModel restaurant)
        {
            var created = await _Restaurants.Update(id, restaurant);
            return Ok(created);
        }

        [Route("api/restaurant/{id}"), HttpDelete]
        // DELETE: api/Restaurant/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _Restaurants.Remove(id);
            return StatusCode(HttpStatusCode.Gone);
        }

        [Route("api/restaurant/{id}/approve"), HttpPost]
        // PUT: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Approve(int id)
        {
            await _Restaurants.Approve(id);
            return Ok();
        }

        [Route("api/restaurant/{id}/reject"), HttpPost]
        // PUT: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Reject(int id)
        {
            await _Restaurants.Reject(id);
            return Ok();
        }
    }
}
