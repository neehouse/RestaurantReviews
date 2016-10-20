(function () {
    app.component('header', {
        templateUrl: '/components/_layout/header.html',
        controller: controller,
        controllerAs: 'header'
    });

    controller.$inject = ['accountService', '$location', '$user'];

    function controller(accountService, $location, $user) {
        var vm = angular.extend(this, {
            logout: logout,
            $user: $user
        });

        function logout() {
            accountService.logout()
                .then(function (response) {
                    $location.path('/');
                }, function (response) {

                });
        }
    }
})();