(function () {
    app.component('reviewForm', {
        templateUrl: '/components/reviews/reviewForm.html',
        controller: controller,
        controllerAs: 'reviewForm',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        }
    });

    controller.$inject = ['ReviewService'];
    function controller(ReviewService) {
        var vm = angular.extend(this, {
            submit: submit,
            cancel: cancel
        });

        function submit(review) {
            ReviewService.create(this.resolve.restaurantId, review);
            //vm.resolve.restaurantId;
            vm.close({ $value: review });
        }

        function cancel() {
            vm.dismiss({$value: 'cancel'});
        }
    }
})();