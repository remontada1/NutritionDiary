app.service('crudService', function ($http){

this.getCustomer = function () {
    return $http.get("/api/customer");
}

});