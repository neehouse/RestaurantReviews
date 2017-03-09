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
            return $http.get('/api/restaurants/' + id);
        }

        function getByCity(city) {
            return $http.get('/api/restaurants', { params: { city: city } });
        }

        function getCities(search) {
            return $http.get('/api/restaurants/cities', { params: { search: search } });
        }

        function create(restaurant) {
            return $http.post('/api/restaurants', restaurant);
        }

        function update(id, restaurant) {
            return $http.put('api/restaurants/' + id, restaurant);
        }

        function remove(id) {
            return $http.delete('api/restaurants/' + id);
        }

        function approve(id) {
            return $http.post('api/restaurants/' + id + '/approve');
        }

        function reject(id) {
            return $http.post('api/restaurants/' + id + '/reject');
        }
    }
})();
