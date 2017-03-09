(function () {
    app.component('restaurantView', {
        templateUrl: '/components/restaurants/restaurantView.html',
        controller: controller,
        controllerAs: 'restaurantView'
    });

    controller.$inject = ['$routeParams', 'RestaurantService', 'ReviewService', '$uibModal', '$account'];
    function controller($routeParams, RestaurantService, ReviewService, $uibModal, $account) {
        var vm = angular.extend(this, {
            $onInit: $onInit,
            addNewReview: addNewReview,

            restaurant: {},
            reviews: []
        });

        vm.$account = $account;

        var restaurantId = $routeParams.id;

        function $onInit() {
            if (restaurantId) getRestaurant();
        }

        function getRestaurant() {
            RestaurantService.get(restaurantId)
                .then(function(response) {
                    vm.restaurant = response.data;
                    getRestaurantReviews();
                }, function (response) {
                    // handle not found.
                });
        }

        function getRestaurantReviews() {
            ReviewService.getByRestaurant(restaurantId)
                .then(function (response) {
                    vm.reviews = response.data;
                }, function (response) {
                    // handle not found.
                });
        }

        function addNewReview() {
            var modal = $uibModal.open({
                component: 'review-form',
                resolve: {
                    restaurantId: function () { return restaurantId; },
                    view: function () { return 'add'; }
                }
            });

            modal.result.then(function(val) {
                getRestaurantReviews();
            });
        }
    }
})();