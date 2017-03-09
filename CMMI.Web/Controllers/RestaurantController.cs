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
using Microsoft.ApplicationInsights.WindowsServer;

namespace CMMI.Web.Controllers
{
    [Authorize]
    public class RestaurantController : BaseApiController
    {
        private readonly Restaurants _restaurants;
        public RestaurantController()
        {
            _restaurants = new Restaurants();
        }

        [AllowAnonymous]
        [Route("api/restaurants"), HttpGet]
        // GET: api/Restaurant
        public async Task<IHttpActionResult> Get(string city = null)
        {
            var restaurants = await _restaurants.GetRestaurantsGroupedByCity(city);
            return Ok(restaurants);
        }

        [AllowAnonymous]
        [Route("api/restaurants/cities"), HttpGet]
        // GET: api/Restaurant
        public async Task<IHttpActionResult> GetCities(string search)
        {
            var cities = await _restaurants.GetCityTypeAhead(search);
            return Ok(cities);
        }

        [AllowAnonymous]
        [Route("api/restaurants/{restaurantId}"), HttpGet]
        // GET: api/Restaurant/5
        public async Task<IHttpActionResult> Get(ApiId restaurantId)
        {
            var restaurant = await _restaurants.GetRestaurant(restaurantId);
            return Ok(restaurant);
        }

        [Route("api/restaurants"), HttpPost]
        // POST: api/Restaurant
        public async Task<IHttpActionResult> Post([FromBody] RestaurantBindingModel restaurant)
        {
            var created = await _restaurants.Create(restaurant);
            return Created("api/restaurants/" + created.Id, created);
        }

        [Route("api/restaurants/{restaurantId}"), HttpPut]
        // PUT: api/Restaurant/5
        public async Task<IHttpActionResult> Put(ApiId restaurantId, [FromBody] RestaurantBindingModel restaurant)
        {
            var created = await _restaurants.Update(restaurantId, restaurant);
            return Ok(created);
        }

        [Route("api/restaurants/{restaurantId}"), HttpDelete]
        // DELETE: api/Restaurant/5
        public async Task<IHttpActionResult> Delete(ApiId restaurantId)
        {
            await _restaurants.Remove(restaurantId);
            return StatusCode(HttpStatusCode.Gone);
        }

        [Route("api/restaurants/{restaurantId}/approve"), HttpPost]
        // PUT: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Approve(ApiId restaurantId)
        {
            await _restaurants.Approve(restaurantId);
            return Ok();
        }

        [Route("api/restaurants/{restaurantId}/reject"), HttpPost]
        // PUT: api/Restaurant/5/approve
        public async Task<IHttpActionResult> Reject(ApiId restaurantId)
        {
            await _restaurants.Reject(restaurantId);
            return Ok();
        }
    }
}
