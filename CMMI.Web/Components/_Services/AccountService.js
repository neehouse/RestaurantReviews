(function () {
    app.service('accountService', service);

    service.$inject = ['$rootScope', '$http', '$account'];
    function service($rootScope, $http, $account) {
        var anonymous = {
            userName: 'Anonymous'
        };

        $init();

        return {
            login: login,
            register: register,
            logout: logout,
            current: current
        };

        function $init() {
            $rootScope.$on('security.unauthorized', function (data) {
                $account.$user = null;
            });
            window.onbeforeunload = saveAccount;
            loadAccount();
        }

        function loadAccount() {
            $account.$user = JSON.parse(sessionStorage.getItem('$user'));
            sessionStorage.removeItem('$user');
        }

        function saveAccount(e) {
            if ($account.$user) sessionStorage.setItem('$user', JSON.stringify($account.$user));
        }

        function current() {
            return $account.$user || anonymous;
        }

        function login(credentials) {
            angular.extend(credentials, { grant_type: 'password' });

            var data = objectToUriString(credentials);

            var promise = $http({ method: 'POST', url: '/token', data: data });

            promise.then(function (response) {
                $account.$user = response.data;
            }, function (response) {

            });

            return promise;
        }

        function objectToUriString(obj) {
            var str = [];
            for (var p in obj)
                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
            return str.join("&");
        }

        function register(user) {
            var promise = $http.post('/api/account/register');

            promise.then(function (response) {
                login({ userName: user.email, password: user.password });
            }, function (response) {

            });

            return promise;
        }

        function logout() {
            var promise = $http.post('/api/account/logout');

            promise.then(function () {
                $account.$user = null;
            }, function () {
                // handle logout error.
            });

            return promise;
        }
    }
})();
