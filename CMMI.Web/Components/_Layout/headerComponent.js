(function () {
    app.component('header', {
        templateUrl: '/components/_layout/header.html',
        controller: controller,
        controllerAs: 'header'
    });

    controller.$inject = ['accountService', '$location', '$account', '$uibModal', '$route'];

    function controller(accountService, $location, $account, $uibModal, $route) {
        var vm = angular.extend(this, {
            logout: logout,
            register: register
        });

        vm.$account = $account;

        function register() {
            var modal = $uibModal.open({
                component: 'register'
            });

            modal.result.then(function(value) {
                $route.reload();
            }, function(value) {

            });
        }

        function logout() {
            accountService.logout()
                .then(function (response) {
                    $location.path('/');
                }, function (response) {

                });
        }
    }
})();