(function () {
    app.service('accountService', service);

    service.$inject = ['$rootScope', '$http', '$user'];
    function service($rootScope, $http, $user) {
        var anonymous = {
            username: 'Anonymous',
            anonymous: true
        };

        $init();

        return {
            login: login,
            register: register,
            logout: logout,
            current: current
        };

        function $init() {
            $rootScope.$on('security.unauthorized', function(data) {
                $user = null;
            });
        }

        function current() {
            return $user || anonymous;
        }

        function login(credentials) {
            angular.extend(credentials, { grant_type: 'password' });

            var data = objectToUriString(credentials);

            var promise = $http({ method: 'POST', url: '/token', data: data });

            promise.then(function(response) {
                $user = response.data;
            }, function(response) {

            });

            return promise;
        }

        function objectToUriString(obj) {
            var str = [];
            for (var p in obj)
                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
            return str.join("&");
        }

        function register(data) {
            
        }

        function logout() {
            var promise = $http.post('/api/account/logout');

            promise.then(function() {
                $user = null;
            }, function() {
                // handle logout error.
            });

            return promise;
        }
    }
})();
