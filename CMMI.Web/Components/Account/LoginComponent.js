(function () {
    app.component('login', {
        templateUrl: '/components/account/login.html',
        controller: controller,
        controllerAs: 'login'
    });

    controller.$inject = ['accountService', '$location'];
    function controller(accountService, $location) {
        var vm = angular.extend(this, {
            login: login,
            showError: false,
            errorMessage: null
        });

        function login(credentials) {
            accountService.login(credentials)
                .then(function(response) {
                    $location.path('/');
                }, function (response) {
                    vm.showError = true;
                    vm.errorMessage = response.data.error_description;
                });
        }
    }
})();