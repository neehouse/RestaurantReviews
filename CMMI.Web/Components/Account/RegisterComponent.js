(function () {
    app.component('register', {
        templateUrl: '/components/account/register.html',
        controller: controller,
        controllerAs: 'register',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        }
    });

    controller.$inject = ['accountService'];
    function controller(accountService) {
        var vm = angular.extend(this, {
            submit: submit,
            cancel: cancel
        });

        function submit(user) {
            accountService.register(user).then(function() {
                vm.close();
            });
        }

        function cancel() {
            vm.dismiss({ $value: 'cancel' });
        }
    }
})();