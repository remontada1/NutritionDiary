var app = angular.module('adventureModule', []);
app.factory('userService', function ($http) {
    var factory = {};
    factory.getallrecords = function () {
        return $http.get('api/Customer/GetAll');
    }
    return factory;
});

