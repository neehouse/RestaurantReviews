(function () {
    app.component('restaurantList', {
        templateUrl: '/components/restaurants/restaurantList.html',
        controller: controller,
        controllerAs: 'restaurantList'
    });

    controller.$inject = ['RestaurantService', '$uibModal', '$location'];
    function controller(RestaurantService, $uibModal, $location) {
        var vm = angular.extend(this, {
            $onInit: $onInit,
            getCities: getCities,
            citySelected: citySelected,
            addNewRestaurant: addNewRestaurant,

            restaurants: [],
            loading: false,
            noResults: false
        });

        function $onInit() {
        }

        function getCities(search) {
            return RestaurantService.getCities(search)
                .then(function (response) {
                    return response.data;
                }, function(response) {
                    
                });
        }

        function citySelected($item, $model, $label, $event) {
            RestaurantService.getByCity(vm.city)
                .then(function (response) {
                    vm.restaurants = response.data;
                }, function(response) {
                    vm.restaurants = [];
                });
        }

        function addNewRestaurant() {
            var modal = $uibModal.open({
                component: 'restaurant-form',
                resolve: {}
            });

            modal.result.then(function (restaurant) {
                $location.path('/restaurant/' + restaurant.Id);
            });
        }
    }
})();