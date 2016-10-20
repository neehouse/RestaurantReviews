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

        function getByRestaurant(id) {
            return $http.get('api/restaurant/' + id + '/reviews');
        }

        function create(review) {

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
