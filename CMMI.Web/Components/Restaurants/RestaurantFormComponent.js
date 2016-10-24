(function () {
    app.component('restaurantForm', {
        templateUrl: '/components/restaurants/restaurantForm.html',
        controller: controller,
        controllerAs: 'restaurantForm',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        }
    });

    controller.$inject = ['RestaurantService'];

    function controller(RestaurantService) {
        var vm = angular.extend(this, {
            submit: submit,
            cancel: cancel,
            getCities: getCities
        });

        function submit(review) {
            // post new restaurant.
            RestaurantService.create(review)
                .then(function(result) {
                    vm.close({ $value: result.data });
                }, function (response) {
                    // handle error.
                });
        }

        function cancel() {
            vm.dismiss({ $value: 'cancel' });
        }

        function getCities(search) {
            return RestaurantService.getCities(search)
                .then(function (response) {
                    return response.data;
                }, function (response) {

                });
        }

    }
})();