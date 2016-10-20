(function () {
    app.component('restaurantView', {
        templateUrl: '/components/restaurants/restaurantView.html',
        controller: controller,
        controllerAs: 'restaurantView'
    });

    controller.$inject = ['$routeParams', 'RestaurantService', 'ReviewService'];
    function controller($routeParams, RestaurantService, ReviewService) {
        var vm = angular.extend(this, {
            $onInit: $onInit,

            restaurant: {},
            reviews: []
        });

        function $onInit() {
            if ($routeParams.id) {
                getRestaurant($routeParams.id);
            }
        }

        function getRestaurant(id) {
            RestaurantService.get(id)
                .then(function(response) {
                    vm.restaurant = response.data;

                    ReviewService.getByRestaurant(id)
                        .then(function(response) {
                            vm.reviews = response.data;
                        }, function(response) {
                            // handle not found.
                        });
                }, function(response) {
                    // handle not found.
                });
        }
    }
})();