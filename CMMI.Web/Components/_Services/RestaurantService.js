(function () {
    app.service('RestaurantService', service);

    service.$inject = ['$http'];
    function service($http) {
        return {
            get: get,
            getByCity: getByCity,
            getCities: getCities,
            create: create,
            update: update,
            remove: remove,
            approve: approve,
            reject: reject
        };

        function get(id) {
            return $http.get('/api/restaurant/' + id);
        }

        function getByCity(city) {
            return $http.get('/api/restaurant', { params: { city: city } });
        }

        function getCities(search) {
            return $http.get('/api/restaurant/cities', { params: { search: search } });
        }

        function create(restaurant) {
            return $http.post('/api/restaurant', restaurant);
        }

        function update(id, review) {

        }

        function remove(id) {
            
        }

        function approve(id) {
            
        }

        function reject(id) {
            
        }
    }
})();
