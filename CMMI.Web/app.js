var app;
(function () {
    app = angular.module('CMMI.Restaurants', ['ngRoute', 'ui.bootstrap']);

    app.constant('$user', null);

    app.factory('securityInterceptor', interceptor);
    interceptor.$inject = ['$q', '$user', '$location'];
    function interceptor($q, $user, $location) {
        return {
            request: request,
            response: response,
            responseError: responseError
        };

        function request(config) {
            config.headers = config.headers || {};
            if ($user && $user.access_token) {
                config.headers.Authorization = $user.token_type + ' ' + $user.access_token;
            }
            return config;
        }

        function response(data) {
            return data || $q.when(data);
        }

        function responseError(data) {
            switch (data.status) {
                case 401: // unauthorized
                    $location.path('/account/login');
                    return $q.reject(data);
                    break;
                default:
                    return $q.reject(data);
                    break;
            }
        }
    }


    app.config(config);
    config.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];
    function config($routeProvider, $httpProvider, $locationProvider) {
        $locationProvider.html5Mode(true);

        $routeProvider
            .when("/", {
                redirectTo: '/restaurant'
            })
            .when("/account/login", {
                allowAnnonymous: true,
                template: '<login></login>'
            })
            .when('/account/register', {
                allowAnnonymous: true,
                template: '<register></register>'
            })
            .when('/restaurant', {
                allowAnnonymous: true,
                template: '<restaurant-list></restaurant-list>'
            })
            .when('/restaurant/new', {
                template: '<restaurant-form></restaurant-form>'
            })
            .when('/restaurant/:id', {
                allowAnnonymous: true,
                template: '<restaurant-view></restaurant-view>'
            })
            .when('/restaurant/:id/review', {
                template: '<restaurant-view></restaurant-view>'
            })
            .when('/user/:id', {
                template: '<user-view></user-view>'
            })
            //.otherwise({
            //    redirectTo: '/restaurant' 
            //})
            ;

        $httpProvider.interceptors.push('securityInterceptor');
    }

})();