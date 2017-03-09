(function() {
    app.service('ReviewService', service);

    service.$inject = ['$http'];
    function service($http) {
        return {
            get: get,
            getByRestaurant: getByRestaurant,
            create: create,
            update: update,
            remove: remove,
            approve: approve,
            reject: reject
        };

        function get(id) {

        }

        function getByRestaurant(restaurantId) {
            return $http.get('api/restaurants/' + restaurantId + '/reviews');
        }

        function create(restaurantId, review) {
            return $http.post('api/restaurants/' + restaurantId + '/reviews', review);
        }

        function update(id, review) {
            return $http.put('api/reviews/' + id, review);
        }

        function remove(id) {
            return $http.delete('api/reviews/' + id);
        }

        function approve(id) {
            return $http.post('api/reviews/' + id + '/approve');
        }

        function reject(id) {
            return $http.post('api/reviews/' + id + '/reject');
        }
    }
})();
